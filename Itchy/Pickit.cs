using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itchy.D2Enums;

namespace Itchy
{
    using PickitDictionary = ConcurrentDictionary<uint, ItemActionInfo>;

    public enum PickitDisableReason
    {
        None = 0,
        InventoryFull = 1,
        Died = 2,
    }

    public class Pickit
    {
        private static double pickupRadius = 10;
        private static double telePickupRadius = 35;
        private static int maxPickTries = 5;
        private static int pickDelay = 500;

        protected D2Game game;
        protected PickitDictionary itemsToPick = new PickitDictionary();
        protected Thread th = null;
        protected volatile bool needStop = false;

        public volatile bool isDisabled = false;
        protected ushort preTeleX = 0;
        protected ushort preTeleY = 0;

        public Pickit(D2Game game)
        {
            this.game = game;

            th = new Thread(() => Loop());
            th.Start();
        }

        public void Stop()
        {
            needStop = true;
            if (th != null)
                th.Join();
        }

        public void Reset()
        {
            preTeleX = 0;
            preTeleY = 0;

            itemsToPick.Clear();
            isDisabled = false;
        }

        public void ToggleEnabled()
        {
            if (isDisabled)
                Enable();
            else
                Disable(PickitDisableReason.None);
        }

        public void Enable()
        {
            if (!isDisabled)
                return;

            isDisabled = false;

            game.LogWarning("Pickit: Pickit enabled.");
        }

        public void Disable(PickitDisableReason reason)
        {
            if (isDisabled)
                return;

            var str = "Pickit: ";
            if (reason == PickitDisableReason.Died)
                str += "You have died! ";
            else if (reason == PickitDisableReason.InventoryFull)
                str += "Intentory is full! ";

            str += "Disabling pickit.";

            if (reason != PickitDisableReason.None)
                if (game.Settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key != Keys.None)
                    str += " Press " + game.Settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key.ToString() +
                        " to reenable pickit.";

            game.LogWarning(str);
            isDisabled = true;
        }

        public void AddPendingItem(ItemActionInfo item)
        {
            item.pickTryCount = 0;
            itemsToPick[item.uid] = item;
        }

        public void OnItemRemovedFromGround(uint uid)
        {
            itemsToPick.Remove(uid);
        }

        protected bool DoTelekinesis(uint uid)
        {
            return game.CastSkillOnTarget(SkillType.Telekinesis, UnitType.Item, uid);
        }

        protected bool TelePick(ushort x, ushort y)
        {
            using (var suspender = new GameSuspender(game))
            {
                if (!game.InGame || !game.GameReady())
                    throw new Exception("Out of game");

                if (game.IsDead())
                {
                    Disable(PickitDisableReason.Died);
                    throw new Exception("Player is dead");
                }

                if (game.IsInTown())
                    return false;

                if (!game.TeleportTo(x, y))
                    return false;

                preTeleX = game.CurrentX;
                preTeleY = game.CurrentY;
            }

            return true;
        }

        protected void TeleBack()
        {
            if (!game.InGame)
                throw new Exception("Out of game");

            if (preTeleX == 0 && preTeleY == 0)
                return;

            using (var suspender = new GameSuspender(game))
            {
                if (!game.GameReady())
                    throw new Exception("Out of game");

                if (game.IsDead())
                {
                    Disable(PickitDisableReason.Died);
                    throw new Exception("Player is dead");
                }

                game.TeleportTo(preTeleX, preTeleY);
            }

            preTeleX = 0;
            preTeleY = 0;
        }

        protected bool Pick(ItemActionInfo item, bool pick, bool telekinesis)
        {
            if (!game.InGame)
                throw new Exception("Out of game");

            using (var suspender = new GameSuspender(game))
            {
                if (!game.GameReady())
                    throw new Exception("Out of game");

                if (game.IsDead())
                {
                    Disable(PickitDisableReason.Died);
                    throw new Exception("Player is dead");
                }

                if (telekinesis && DoTelekinesis(item.uid))
                { }
                else if (pick)
                    game.PickItem(item.uid);
                else
                {
                    --item.pickTryCount;
                    return false;
                }
            }

            return true;
        }

        protected ItemActionInfo GetClosestItem(double range = double.MaxValue)
        {
            ItemActionInfo temp = null;
            double dist = range;

            foreach (var item in itemsToPick)
            {
                if (item.Value.dropTime.MSecToNow() <= pickDelay)
                    continue;

                var newDist = item.Value.Distance(game.CurrentX, game.CurrentY);
                if (newDist < dist)
                {
                    temp = item.Value;
                    dist = newDist;
                }
            }

            return temp;
        }

        public void Loop()
        {
            while (!needStop)
            {
                Thread.Sleep(50);

                if (!game.InGame ||
                    !game.Settings.ReceivePacketHack.Enabled ||
                    !game.Settings.ReceivePacketHack.ItemTracker.Enabled ||
                    !game.Settings.ReceivePacketHack.ItemTracker.EnablePickit ||
                    isDisabled)
                    continue;

                try
                {
                    var item = GetClosestItem(pickupRadius);
                    if (item != null)
                    {
                        ++item.pickTryCount;
                        if (item.pickTryCount >= maxPickTries)
                        {
                            game.Log("Pickit: failed to pick {0} after {1} retries", item.uid, item.pickTryCount);
                            itemsToPick.Remove(item.uid);
                        }
                        else if (Pick(item, true, false))
                            Thread.Sleep(pickDelay);
                        continue;
                    }

                    item = GetClosestItem(telePickupRadius);
                    if (item != null)
                    {
                        if (game.Settings.ReceivePacketHack.ItemTracker.UseTelekinesis && item.info.CanBeTelekinesised())
                        {
                            ++item.pickTryCount;
                            if (item.pickTryCount >= maxPickTries)
                            {
                                game.Log("Pickit: failed to pick {0} after {1} retries", item.uid, item.pickTryCount);
                                itemsToPick.Remove(item.uid);
                            }
                            else if (Pick(item, false, true))
                            {
                                Thread.Sleep(pickDelay);
                                continue;
                            }
                        }
                        if (preTeleX == 0 && preTeleY == 0 && game.Settings.ReceivePacketHack.ItemTracker.EnableTelepick &&
                            item.processingInfo.Find(it => it.NoTele) == null &&
                            TelePick((ushort)item.x, (ushort)item.y))
                        {
                            Thread.Sleep(400);
                            continue;
                        }
                    }

                    if (game.Settings.ReceivePacketHack.ItemTracker.EnableTelepick &&
                        game.Settings.ReceivePacketHack.ItemTracker.TeleBack &&
                        preTeleX != 0 && preTeleY != 0)
                    {
                        TeleBack();
                        Thread.Sleep(400);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
