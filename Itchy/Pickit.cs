using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itchy
{
    using PickitDictionary = ConcurrentDictionary<uint, ItemActionInfo>;

    public class Pickit
    {
        public PickitDictionary ItemsToPick { get { return itemsToPick; } }

        private static double pickupRadius = 10;
        private static double telePickupRadius = 35;

        private static int maxPickTries = 5;
        private static int pickDelay = 500;

        protected volatile uint telepickingUid = 0;
        protected D2Game game;
        protected PickitDictionary itemsToPick = new PickitDictionary();
        protected Thread th = null;
        protected volatile bool needStop = false;
        public volatile bool fullInventory = false;

        protected ushort preTeleX = 0;
        protected ushort preTeleY = 0;

        public Pickit(D2Game game)
        {
            this.game = game;

            th = new Thread(() =>
            {
                while (!needStop)
                {
                    ProcessPicks(false);
                    Thread.Sleep(150);
                }
            });

            th.Start();
        }

        public void Reset(bool reenable = false)
        {
            preTeleX = 0;
            preTeleY = 0;

            telepickingUid = 0;
            if (!reenable)
                itemsToPick.Clear();
            fullInventory = false;

            if (reenable)
            {
                Task.Factory.StartNew(() => ProcessPicks(true));
                game.Log("Pickit reset.");
            }
        }

        public void Stop()
        {
            needStop = true;
            if (th != null)
                th.Join();
        }

        public void FullIntentory()
        {
            var str = "Intentory is full! Disabling pickit.";
            if (game.Settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key != Keys.None)
                str += " Press " + game.Settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key.ToString() +
                    " to reenable pickit.";

            game.LogWarning(str);
            fullInventory = true;
            //itemsToPick.Clear();
            telepickingUid = 0;
        }

        public void AddPendingItem(ItemActionInfo item)
        {
            item.pickTryCount = 0;
            itemsToPick[item.uid] = item;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                ProcessPicks(true);
            });
        }

        public void RemoveItem(uint uid)
        {
            itemsToPick.Remove(uid);

            if (uid == telepickingUid)
            {
                telepickingUid = 0;
                if (game.Settings.ReceivePacketHack.ItemTracker.TeleBack &&
                    preTeleX != 0 && preTeleY != 0)
                {
                    TeleportTo(preTeleX, preTeleY);
                }
            }
        }

        public void ProcessPicks(bool firstTry)
        {
            if (!game.InGame || !game.Settings.ReceivePacketHack.ItemTracker.EnablePickit || !game.Settings.ReceivePacketHack.ItemTracker.Enabled || fullInventory)
                return;

            var items = itemsToPick.ToArray().Where(it => (firstTry ? it.Value.pickTryCount == 0 : it.Value.pickTryCount != 0) || it.Value.uid == telepickingUid);

            try
            {
                foreach (var item in items)
                {
                    var i = item.Value;
                    var dist = i.DistanceSq(game.CurrentX, game.CurrentY);

                    var pickRadius = pickupRadius;
                    i.processingInfo.ForEach(it => pickRadius = Math.Min(pickRadius, it.PickRadius));
                    if (pickRadius == 0)
                        pickRadius = pickupRadius;

                    bool canBeTelekinesised = dist <= Math.Pow(telePickupRadius, 2) && game.Settings.ReceivePacketHack.ItemTracker.UseTelekinesis &&
                        i.info.CanBeTelekinesised() && dist >= Math.Pow(telePickupRadius, 2);

                    if (dist <= Math.Pow(pickRadius, 2) || canBeTelekinesised)
                    {
                        if (i.pickTryCount == 0)
                        {
                            ++i.pickTryCount;
                            i.pickDate = DateTime.Now;
                            if (Pick(i.uid, canBeTelekinesised))
                                continue;
                        }
                        else if (i.pickDate.MSecToNow() >= pickDelay)
                        {
                            if (++i.pickTryCount > maxPickTries)
                            {
                                game.Log("Failed to pick {0} after {1} tries", i.uid, i.pickTryCount - 1);
                                itemsToPick.Remove(i.uid);
                                continue;
                            }

                            game.Log("Retrying to pick {0} #{1}", i.uid, i.pickTryCount);
                            i.pickDate = DateTime.Now;
                            if (Pick(i.uid, canBeTelekinesised))
                                continue;
                        }
                    }

                    if (dist <= Math.Pow(telePickupRadius, 2) && telepickingUid == 0 && game.Settings.ReceivePacketHack.ItemTracker.EnableTelepick &&
                        i.processingInfo.Find(it => it.NoTele) == null)
                    {
                        if (TelePick(i.uid, (ushort)i.x, (ushort)i.y))
                            continue;
                    }

                    i.pickTryCount = 0;
                }
            }
            catch (Exception) { return; }
        }

        protected bool TeleportTo(ushort x, ushort y)
        {
            return game.CastSkillXY(SkillType.Teleport, x, y);
        }

        protected bool DoTelekinesis(uint uid)
        {
            return game.CastSkillOnTarget(SkillType.Telekinesis, UnitType.Item, uid);
        }

        public bool TelePick(uint uid, ushort x, ushort y)
        {
            game.SuspendThreads();

            if (!game.InGame || !game.GameReady())
            {
                game.ResumeThreads();
                throw new Exception("Out of game");
            }

            if (game.IsInTown())
            {
                game.ResumeThreads();
                return true;
            }

            if (!TeleportTo(x, y))
            {
                game.ResumeThreads();
                return true;
            }

            preTeleX = game.CurrentX;
            preTeleY = game.CurrentY;

            telepickingUid = uid;

            game.ResumeThreads();
            return true;
        }

        public bool Pick(uint uid, bool telekinesis)
        {
            if (!game.InGame)
                throw new Exception("Out of game");

            game.SuspendThreads();

            if (!game.GameReady())
            {
                game.ResumeThreads();
                throw new Exception("Out of game");
            }

            if (telekinesis)
            {
                if (!DoTelekinesis(uid))
                {
                    game.ResumeThreads();
                    return false;
                }
            }
            else
                game.PickItem(uid);

            game.ResumeThreads();
            return true;
        }
    }
}
