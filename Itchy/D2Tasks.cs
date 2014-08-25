﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public partial class D2Game
    {
        public volatile bool chickening = false;

        protected bool TestPvPFlag(uint dwUnitId1, uint dwUnitId2, uint flag)
        {
            return false;
            using (var asm = new Fasm.ManagedFasm())
            {

                asm.AddLine("push {0}", flag);
                asm.AddLine("mov esi, {0}", dwUnitId2);
                asm.AddLine("mov edx, {0}", dwUnitId1);
                asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + D2Client.TestPvpFlag_I);
                asm.AddLine("ret");

                var ret = pd.ExecuteRemoteCode(asm.Assemble());
                return ret != 0;
            }
        }

        public void ChickenTask(ushort hp, ushort mana, uint relationUnitId)
        {
            if (chickening)
                return;

            if (!GameReady())
                return;

            if (IsInTown())
                return;

            if (relationUnitId != 0)
            {
                if (!Settings.Chicken.ChickenOnHostile)
                    return;

                UnitAny player;
                if (!GetPlayerUnit(out player))
                    return;

                if (!TestPvPFlag(relationUnitId, player.dwUnitId, 8))
                    return;

                if (Settings.Chicken.ChickenToTown)
                    LogWarning("Chickening to town because of hostility");
            }
            else
            {
                if ((hp & 0x8000) != 0)
                    hp ^= 0x8000;
                if ((mana & 0x8000) != 0)
                    mana ^= 0x8000;
                mana <<= 1;

                var unit = GetPlayerUnit();
                var maxLife = GetUnitStat(unit, Stat.MaxHealth);
                var maxMana = GetUnitStat(unit, Stat.MaxMana);
                maxLife >>= 8;
                maxMana >>= 8;

                // dead case
                if (hp == 0)
                    return;

                var hpPct = hp * 100f / maxLife;
                var manaPct = mana * 100f / maxMana;
                if (hpPct < Settings.Chicken.ChickenLifePercent)
                {
                    if (Settings.Chicken.ChickenToTown)
                        LogWarning("Chickening to town at {0:0.00}% health", hpPct);
                }
                else if (manaPct < Settings.Chicken.ChickenManaPercent)
                {
                    if (Settings.Chicken.ChickenToTown)
                        LogWarning("Chickening to town at {0:0.00}% mana", manaPct);
                }
                else
                    return;
            }

            chickening = true;

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

            var now = DateTime.Now;
            while (now.MSecToNow() < 2000 && !IsInTown())
            {
                Thread.Sleep(300);
            }

            if (!IsInTown())
            {
                ExitGame();
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
                        SuspendThreads();
                        try
                        {
                            socketsPerItem[dwItemId] = GetUnitStat(pItem, Stat.Sockets);
                        }
                        catch (Exception) { }

                        ResumeThreads();
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
                        SuspendThreads();

                        try
                        {
                            var item = pd.Read<UnitAny>(pItem);
                            var pUnit = GetPlayerUnit();
                            var diff = pd.ReadByte(pd.GetModuleAddress("d2client.dll") + D2Client.pDifficulty);
                            var pItemPriceList = pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pItemPriceList);

                            var price = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetItemPrice,
                                CallingConventionEx.StdCall,
                                pUnit, pItem, diff, pItemPriceList, 0x9A, 1);

                            pricePerItem[item.dwUnitId] = price;
                        }
                        catch (Exception) { }

                        ResumeThreads();
                        hasPendingTask = false;
                    });
                }
            }

            return val;
        }
    }
}
