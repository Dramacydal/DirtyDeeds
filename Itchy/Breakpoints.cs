using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WhiteMagic;

namespace Itchy
{
    public class D2BreakPoint : HardwareBreakPoint
    {
        public D2Game Game { get; set; }

        public D2BreakPoint(D2Game game, int address, uint len, Condition condition)
            : base(address, len, condition)
        {
            this.Game = game;
        }
    }

    public class LightBreakPoint : D2BreakPoint
    {
        public LightBreakPoint(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax = 0xFF;     // light density
            ctx.Eip += 0xEB;    // skip code

            return true;
        }
    }

    public class RainBreakPoint : D2BreakPoint
    {
        public RainBreakPoint(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Al &= 0;
            ctx.Eip += 4;

            return true;
        }
    }

    // 6FB332FF
    public class ReceivePacketBreakPoint : D2BreakPoint
    {
        public ReceivePacketBreakPoint(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        private static PlayerMode[] allowableModes = new PlayerMode[] {PlayerMode.Death, PlayerMode.Stand_OutTown,PlayerMode.Walk_OutTown,PlayerMode.Run,PlayerMode.Stand_InTown,PlayerMode.Walk_InTown,
                            PlayerMode.Dead,PlayerMode.Sequence,PlayerMode.Being_Knockback};

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            bool skip = false;

            var pPacket = ctx.Edi;
            var len = ctx.Edx;

            var packet = pd.ReadBytes(pPacket, (int)len);

            //MessageBox.Show(packet.ToString());

            switch (packet[0])
            {
                case 0xE:
                {
                    if (!Game.Settings.ReceivePacketHack.FastPortal)
                        break;

                    // no portal anim #1
                    var pUnit = Game.FindUnit(BitConverter.ToUInt32(packet, 2), (UnitType)packet[1]);
                    if (pUnit != 0)
                    {
                        var unit = pd.Read<UnitAny>(pUnit);
                        if (unit.dwTxtFileNo == 0x3B)
                            skip = true;
                    }

                    break;
                }
                case 0x15:
                {
                    if (!Game.Settings.ReceivePacketHack.BlockFlash &&
                        !Game.Settings.ReceivePacketHack.FastTele)
                        break;
                    
                    var pPlayer = pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pPlayerUnit);
                    if (pPlayer == 0)
                        break;

                    var unit = pd.Read<UnitAny>(pPlayer);
                    if (BitConverter.ToUInt32(packet, 2) == unit.dwUnitId)
                    {
                        // no flash
                        if (Game.Settings.ReceivePacketHack.BlockFlash)
                            pd.WriteByte(pPacket + 10, 0);

                        // fast teleport
                        if (Game.Settings.ReceivePacketHack.FastTele && !allowableModes.Contains((PlayerMode)unit.dwMode))
                        {
                            unit.dwFrameRemain = 0;
                            pd.Write<UnitAny>(pPlayer, unit);
                        }
                    }
                    break;
                }
                case 0x18:
                case 0x95:
                {
                    if (!Game.Settings.Chicken.Enabled || Game.chickening)
                        break;

                    var life = BitConverter.ToUInt16(packet, 1);
                    var mana = BitConverter.ToUInt16(packet, 3);
                    Task.Factory.StartNew(() => Game.ChickenTask(life, mana, false));
                    break;
                }
                case 0x51:
                {
                    if ((UnitType)packet[1] == UnitType.Object && BitConverter.ToUInt16(packet, 6) == 0x3B)
                    {
                        // no portal anim #2
                        if (Game.Settings.ReceivePacketHack.FastPortal)
                            pd.WriteByte(pPacket + 12, 2);

                        UnitAny unit;
                        if (Game.backToTown && Game.GetPlayerUnit(out unit))
                        {
                            if (!Game.IsInTown())
                            {
                                var path = pd.Read<Path>(unit.pPath);
                                if (Misc.Distance(path.xPos, path.yPos, BitConverter.ToUInt16(packet, 8), BitConverter.ToUInt16(packet, 10)) < 10)
                                    Task.Factory.StartNew(() => Game.Interact(BitConverter.ToUInt32(packet, 2), UnitType.Object));
                            }
                            Game.backToTown = false;
                        }
                    }
                    break;
                }
                case 0x5A:  // event message
                {
                    var mode = packet[1];
                    if (mode == 0x7)    // player relation
                    {
                        if (!Game.Settings.Chicken.Enabled || Game.chickening)
                            break;

                        var dwUnitId = BitConverter.ToUInt32(packet, 3);
                        var pRosterUnit = pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pPlayerUnitList);
                        for (; pRosterUnit != 0; )
                        {
                            var rUnit = pd.Read<RosterUnit>(pRosterUnit);
                            if (rUnit.dwUnitId == dwUnitId)
                            {
                                Task.Factory.StartNew(() => Game.ChickenTask(0, 0, true));
                                break;
                            }
                            pRosterUnit = rUnit.pNext;
                        }
                    }
                    break;
                }
                case 0xA7:
                {
                    // skip delay between entering portals
                    if (Game.Settings.ReceivePacketHack.FastPortal && packet[6] == 102)
                        skip = true;
                    break;
                }
            }
            
            if (!skip)
            {
                ctx.Ecx = ctx.Edi;
                ctx.Eip += 2;
            }
            else
            {
                ctx.Esp += 4;
                ctx.Eip += 7;
                ctx.Eax = 0;
            }

            return true;
        }
    }

    // 1.12 0xAC236 6FB5C236
    // 1.13d 0x96736 6FB46736
    public class ItemNameBreakPoint : D2BreakPoint
    {
        public ItemNameBreakPoint(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Al = pd.ReadByte(ctx.Ebp + 0x12A);
            ctx.Eip += 6;

            var pString = ctx.Edi;
            var pItem = ctx.Ebx;
            var str = pd.ReadUTF16String(pString);
            //"ÿc";
            //str = "ÿc1" + str;

            var item = pd.Read<UnitAny>(pItem);
            var itemData = pd.Read<ItemData>(item.pItemData);

            var pTxt = Game.GetItemText(item.dwTxtFileNo);
            var txt = pd.Read<ItemTxt>(pTxt);
            var dwCode = txt.GetDwCode();
            var code = txt.GetCode();

            bool changed = false;

            if (Game.Settings.ItemName.ShowEth && (itemData.dwFlags & 0x400000) != 0)
            {
                if (!changed)
                    str += " ";
                str += "{E}";
                changed = true;
            }

            var runeNumber = item.RuneNumber();
            if (runeNumber > 0 && Game.Settings.ItemName.ShowRuneNumber)
            {
                if (!changed)
                    str += " ";
                str += "(" + runeNumber + ")";
                changed = true;
            }

            if (Game.Settings.ItemName.ShowSockets)
            {
                var cnt = Game.GetItemSockets(pItem, item.dwUnitId);

                if (cnt > 0 && cnt != uint.MaxValue)
                {
                    if (!changed)
                        str += " ";
                    str += "(" + cnt + ")";
                    changed = true;
                }
            }

            if (Game.Settings.ItemName.ShowItemLevel && itemData.dwItemLevel > 1)
            {
                if (!changed)
                    str += " ";
                str += "(L" + itemData.dwItemLevel + ")";
                changed = true;
            }

            if (Game.Settings.ItemName.ShowItemPrice)
            {
                var price = Game.GetItemPrice(pItem, item.dwUnitId);

                if (price > 0 && price != uint.MaxValue)
                {
                    if (!changed)
                        str += " ";
                    str += "($" + price + ")";
                    changed = true;
                }
            }

            if (Game.Settings.ItemName.ChangeItemColor)
            {
                var itemInfo = Game.ItemStorage.GetInfo(item.dwTxtFileNo);
                if (itemInfo != null)
                {
                    var sock = Game.GetItemSockets(pItem, item.dwUnitId);
                    var configEntry = Game.ItemSettings.GetMatch(itemInfo, txt.GetCode(), sock,
                        (itemData.dwFlags & 0x400000) != 0, (ItemQuality)itemData.dwQuality);
                    if (configEntry != null)
                    {
                        if (configEntry.Color != D2Color.Default)
                        {
                            str = "ÿc" + configEntry.Color.GetCode() + str;
                            changed = true;
                        }
                    }
                }
            }

            if (changed)
                pd.WriteUTF16String(pString, str);

            return true;
        }
    }

    // 1.13 0x997B2 0x6FB497B2
    public class ViewInventoryBp1 : D2BreakPoint
    {
        public ViewInventoryBp1(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Esi = Game.GetViewingUnit();
            ctx.Eip += 6;

            return true;
        }
    }

    // 1.13 0x98E84 0x6FB48E84
    public class ViewInventoryBp2 : D2BreakPoint
    {
        public ViewInventoryBp2(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Ebx = Game.GetViewingUnit();
            ctx.Eip += 6;

            return true;
        }
    }

    // 1.13 0x97E3F 0x6FB47E3F
    // new 0x97E41 0x6FB47E41
    public class ViewInventoryBp3 : D2BreakPoint
    {
        public ViewInventoryBp3(D2Game game, int address, uint len, Condition condition) : base(game, address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            var viewvingUnit = Game.GetViewingUnit();
            ctx.Edi = viewvingUnit;
            ctx.Ecx = pd.ReadUInt(ctx.Edi + 0x60);
            ctx.Eip += 3;

            return true;
        }
    }
}
