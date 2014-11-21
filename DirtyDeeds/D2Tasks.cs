using System;
using System.Threading;
using System.Threading.Tasks;
using DD.D2Enums;
using DD.Log;
using WhiteMagic;
using DD.D2Structs;
using DD.D2Pointers;
using DD.Extensions;

namespace DD
{
    public partial class D2Game
    {
        public volatile bool chickening = false;

        public void TryChicken(ushort hp, ushort mana, bool hostile)
        {
            if (chickening)
                return;

            if (!GameReady())
                return;

            if (IsInTown())
                return;

            if (hostile)
            {
                if (!Settings.Chicken.ChickenOnHostile)
                    return;

                if (Settings.Chicken.ChickenToTown)
                    Logger.Chicken.Log(this, LogType.Warning, "Chickening to town because of hostility.");
            }
            else
            {
                if ((hp & 0x8000) != 0)
                    hp ^= 0x8000;

                // dead case
                if (hp == 0)
                    return;

                if ((mana & 0x8000) != 0)
                    mana ^= 0x8000;
                mana <<= 1;

                var unit = GetPlayerUnit();
                var maxLife = llGetUnitStat(unit, StatType.MaxHealth);
                var maxMana = llGetUnitStat(unit, StatType.MaxMana);
                maxLife >>= 8;
                maxMana >>= 8;

                var hpPct = hp * 100f / maxLife;
                var manaPct = mana * 100f / maxMana;
                if (hpPct < Settings.Chicken.ChickenLifePercent)
                {
                    if (Settings.Chicken.ChickenToTown)
                        Logger.Chicken.Log(this, LogType.Warning, "Chickening to town at {0:0.00}% health.", hpPct);
                }
                else if (manaPct < Settings.Chicken.ChickenManaPercent)
                {
                    if (Settings.Chicken.ChickenToTown)
                        Logger.Chicken.Log(this, LogType.Warning, "Chickening to town at {0:0.00}% mana.", manaPct);
                }
                else
                    return;
            }

            chickening = true;

            Task.Factory.StartNew(() => ChickenTask());
        }

        public void ChickenTask()
        {
            using (var suspender = new GameSuspender(this))
            {
                if (!Settings.Chicken.ChickenToTown)
                {
                    ExitGame();
                    return;
                }

                backToTown = true;

                if (!OpenPortal())
                {
                    backToTown = false;
                    ExitGame();
                    return;
                }
            }

            var now = DateTime.Now;
            while (now.MSecToNow() < 2000)
            {
                if (IsInTown())
                    break;
                Thread.Sleep(300);
            }

            if (!IsInTown())
            {
                using (var suspender = new GameSuspender(this))
                {
                    ExitGame();
                }
                return;
            }

            chickening = false;
        }

        public uint GetItemSockets(uint pItem, uint dwItemId)
        {
            var val = uint.MaxValue;

            if (socketsPerItem.ContainsKey(dwItemId))
                val = socketsPerItem[dwItemId];
            else
            {
                if (!hasPendingTask)
                {
                    hasPendingTask = true;
                    Task.Factory.StartNew(() =>
                    {
                        using (var suspender = new GameSuspender(this))
                        {
                            try
                            {
                                socketsPerItem[dwItemId] = GetUnitStat(pItem, StatType.Sockets);
                            }
                            catch { }
                        }

                        hasPendingTask = false;
                    });
                }
            }

            return val;
        }

        public uint GetItemPrice(uint pItem, uint dwItemId)
        {
            var val = uint.MaxValue;

            if (pricePerItem.ContainsKey(dwItemId))
                val = pricePerItem[dwItemId];
            else
            {
                if (!hasPendingTask)
                {
                    hasPendingTask = true;
                    Task.Factory.StartNew(() =>
                    {
                        using (var suspender = new GameSuspender(this))
                        {
                            try
                            {
                                var item = pd.Read<UnitAny>(pItem);
                                var pUnit = GetPlayerUnit();
                                var diff = GetDifficulty();
                                var pItemPriceList = pd.ReadUInt(D2Client.pItemPriceList);

                                var price = pd.Call(D2Common.GetItemPrice,
                                    CallingConventionEx.StdCall,
                                    pUnit, pItem, diff, pItemPriceList, 0x9A, 1);

                                pricePerItem[item.dwUnitId] = price;
                            }
                            catch { }
                        }

                        hasPendingTask = false;
                    });
                }
            }

            return val;
        }
    }
}
