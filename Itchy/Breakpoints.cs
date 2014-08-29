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
        public string ModuleName { get { return moduleName; } }

        protected string moduleName;

        public D2BreakPoint(D2Game game, string moduleName, int address)
            : base(address, 1, Condition.Code)
        {
            this.Game = game;
            this.moduleName = moduleName;
        }
    }

    public class LightBreakPoint : D2BreakPoint
    {
        public LightBreakPoint(D2Game game) : base(game, "d2client.dll", 0x233A7) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax = 0xFF;     // light density
            ctx.Eip += 0xEB;    // skip code

            return true;
        }
    }

    public class WeatherBreakPoint : D2BreakPoint
    {
        public WeatherBreakPoint(D2Game game) : base(game, "d2common.dll", 0x30C92) { }

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
        public ReceivePacketBreakPoint(D2Game game) : base(game, "d2client.dll", 0x832FF) { }

        private static PlayerMode[] allowableModes = new PlayerMode[] {PlayerMode.Death, PlayerMode.Stand_OutTown,PlayerMode.Walk_OutTown,PlayerMode.Run,PlayerMode.Stand_InTown,PlayerMode.Walk_InTown,
                            PlayerMode.Dead,PlayerMode.Sequence,PlayerMode.Being_Knockback};

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            bool skip = false;

            var pPacket = ctx.Edi;
            var len = ctx.Edx;

            var packet = pd.ReadBytes(pPacket, (int)len);

            //MessageBox.Show(packet.ToString());

            switch ((GameServerPacket)packet[0])
            {
                case GameServerPacket.RemoveGroundUnit:
                {
                    if (!Game.Settings.ReceivePacketHack.ItemTracker.Enabled ||
                        !Game.Settings.ReceivePacketHack.ItemTracker.EnablePickit)
                        break;

                    Game.ItemGoneHandler(packet);
                    break;
                }
                case GameServerPacket.GameObjectModeChange:
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
                case GameServerPacket.PlayerReassign:
                {
                    if (!Game.Settings.ReceivePacketHack.BlockFlash &&
                        !Game.Settings.ReceivePacketHack.FastTele &&
                        !(Game.Settings.ReceivePacketHack.ItemTracker.Enabled &&
                        Game.Settings.ReceivePacketHack.ItemTracker.EnablePickit))
                        break;

                    var unitType = (UnitType)packet[1];
                    if (unitType != UnitType.Player)
                        break;

                    var pPlayer = Game.GetPlayerUnit();
                    if (pPlayer == 0)
                        break;

                    var unit = pd.Read<UnitAny>(pPlayer);
                    if (BitConverter.ToUInt32(packet, 2) == unit.dwUnitId)
                    {
                        if (Game.Settings.ReceivePacketHack.ItemTracker.Enabled && Game.Settings.ReceivePacketHack.ItemTracker.EnablePickit)
                        {
                            Game.CurrentX = BitConverter.ToUInt16(packet, 6);
                            Game.CurrentY = BitConverter.ToUInt16(packet, 8);
                            Task.Factory.StartNew(() => Game.OnRelocaton());
                        }

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
                case GameServerPacket.Unknown18:
                case GameServerPacket.PlayerLifeManaChange:
                {
                    if (!Game.Settings.Chicken.Enabled || Game.chickening)
                        break;

                    var life = BitConverter.ToUInt16(packet, 1);
                    var mana = BitConverter.ToUInt16(packet, 3);
                    Task.Factory.StartNew(() => Game.TryChicken(life, mana, false));
                    break;
                }
                case GameServerPacket.GameObjectAssignment:
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
                case GameServerPacket.PlayerInfomation:  // event message
                {
                    var type = packet[1];
                    var infoType = packet[2];
                    var relationType = packet[7];
                    if (type == 0x7 && infoType == 8 && relationType == 3)
                    {
                        if (!Game.Settings.Chicken.Enabled || !Game.Settings.Chicken.ChickenOnHostile || Game.chickening)
                            break;

                        Task.Factory.StartNew(() => Game.TryChicken(0, 0, true));
                    }
                    break;
                }
                case GameServerPacket.WorldItemAction:
                case GameServerPacket.OwnedItemAction:
                {
                    skip = !Game.ItemActionHandler(packet);
                    break;
                }
                case GameServerPacket.DelayedState:
                {
                    // skip delay between entering portals
                    if (Game.Settings.ReceivePacketHack.FastPortal && packet[6] == 102)
                        skip = true;
                    break;
                }
                case GameServerPacket.TriggerSound:
                {
                    if (Game.Settings.ReceivePacketHack.ItemTracker.Enabled &&
                        Game.Settings.ReceivePacketHack.ItemTracker.EnablePickit)
                    {
                        if (packet[1] == 0 && packet[6] == 0x17)
                            Game.Pickit.FullIntentory();
                    }
                    break;
                }
                case GameServerPacket.AttributeByte:
                case GameServerPacket.AttributeWord:
                case GameServerPacket.AttributeDWord:
                {
                    var attr = (StatType)packet[1];
                    uint val = 0;
                    switch ((GameServerPacket)packet[0])
                    {
                        case GameServerPacket.AttributeByte:
                            val = (uint)packet[2];
                            break;
                        case GameServerPacket.AttributeWord:
                            val = BitConverter.ToUInt16(packet, 2);
                            break;
                        case GameServerPacket.AttributeDWord:
                            val = (uint)((packet[5] << 16) | (packet[4] << 8) | packet[3]);
                            break;
                    }

                    Game.PlayerInfo.SetStat(attr, val);
                    break;
                }
                case GameServerPacket.StateNotification:
                {
                    /*var uid = BitConverter.ToUInt32(packet, 1);
                    var attr = (StatType)packet[5];
                    var value = BitConverter.ToUInt32(packet, 6);

                    UnitAny player;
                    if (!Game.GetPlayerUnit(out player))
                        break;

                    if (player.dwUnitId != uid)
                        break;

                    Game.PlayerInfo.SetStat(attr, value);*/
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
        public ItemNameBreakPoint(D2Game game) : base(game, "d2client.dll", 0x96736) { }

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
            var code = txt.GetCode();

            bool changed = false;

            if (Game.Settings.ItemNameHack.ShowEth && (itemData.dwFlags & 0x400000) != 0)
            {
                if (!changed)
                    str += " ";
                str += "{E}";
                changed = true;
            }

            var runeNumber = item.RuneNumber();
            if (runeNumber > 0 && Game.Settings.ItemNameHack.ShowRuneNumber)
            {
                if (!changed)
                    str += " ";
                str += "(" + runeNumber + ")";
                changed = true;
            }

            if (Game.Settings.ItemNameHack.ShowSockets)
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

            if (Game.Settings.ItemNameHack.ShowItemLevel && itemData.dwItemLevel > 1)
            {
                if (!changed)
                    str += " ";
                str += "(L" + itemData.dwItemLevel + ")";
                changed = true;
            }

            if (Game.Settings.ItemNameHack.ShowItemPrice)
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

            if (Game.Settings.ItemNameHack.ChangeItemColor)
            {
                var itemInfo = ItemStorage.GetInfo(item.dwTxtFileNo);
                if (itemInfo != null)
                {
                    var sock = Game.GetItemSockets(pItem, item.dwUnitId);
                    var configEntries = Game.ItemProcessingSettings.GetMatches(itemInfo, sock,
                        (itemData.dwFlags & 0x400000) != 0, (ItemQuality)itemData.dwQuality).Where(it => it.Color != D2Color.Default);
                    if (configEntries.Count() != 0)
                    {
                        var entry = configEntries.First();
                        if (entry.Color != D2Color.Default)
                        {
                            str = "ÿc" + entry.Color.GetCode() + str;
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
        public ViewInventoryBp1(D2Game game) : base(game, "d2client.dll", 0x997B2) { }

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
        public ViewInventoryBp2(D2Game game) : base(game, "d2client.dll", 0x98E84) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Ebx = Game.GetViewingUnit();
            ctx.Eip += 6;

            return true;
        }
    }

    // 1.13 0x97E3F 0x6FB47E3F
    // new 0x97E41 0x6FB47E41
    // 97E1A 6FB47E1A
    public class ViewInventoryBp3 : D2BreakPoint
    {
        public ViewInventoryBp3(D2Game game) : base(game, "d2client.dll", 0x97E1A) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            var viewingUnit = Game.GetViewingUnit();
            //var val = pd.ReadUInt(ctx.Edi);
            //if (val != 1)
                ctx.Edi = viewingUnit;

            //ctx.Ecx = pd.ReadUInt(ctx.Edi + 0x60);
            //ctx.Eip += 3;

            ctx.Eip += 7;

            return true;
        }
    }

    public class InfravisionBreakPoint : D2BreakPoint
    {
        public InfravisionBreakPoint(D2Game game) : base(game, "d2client.dll", 0xB4A23) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            // Esi - pUnit
            var hide = 0u;

            var pUnit = ctx.Esi;
            if (pUnit == 0)
                hide = 1;
            else if (Game.Settings.Infravision.HideCorpses || Game.Settings.Infravision.HideDying || Game.Settings.Infravision.HideItems)
            {
                var unit = pd.Read<UnitAny>(pUnit);
                switch ((UnitType)unit.dwType)
                {
                    case UnitType.Monster:
                    {
                        if (Game.Settings.Infravision.HideCorpses &&
                            unit.dwMode == (uint)NpcMode.Dead ||
                            Game.Settings.Infravision.HideDying && 
                            unit.dwMode == (uint)NpcMode.Death)
                            hide = 1;
                        break;
                    }
                    case UnitType.Item:
                    {
                        if (!Game.Settings.Infravision.HideItems)
                            break;

                        var itemInfo = ItemStorage.GetInfo(unit.dwTxtFileNo);
                        if (itemInfo != null)
                        {
                            var itemData = pd.Read<ItemData>(unit.pItemData);
                            var pTxt = Game.GetItemText(unit.dwTxtFileNo);
                            var txt = pd.Read<ItemTxt>(pTxt);

                            var sock = Game.GetItemSockets(pUnit, unit.dwUnitId);
                            var configEntries = Game.ItemProcessingSettings.GetMatches(itemInfo, sock,
                                (itemData.dwFlags & 0x400000) != 0, (ItemQuality)itemData.dwQuality).Where(it => it.Hide);
                            if (configEntries.Count() != 0)
                                hide = 1;
                        }
                        break;
                    }
                }
            }

            ctx.Eax = hide;

            ctx.Eip += 0x77;

            return true;
        }
    }
}
