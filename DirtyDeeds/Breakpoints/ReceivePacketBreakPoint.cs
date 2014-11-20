using DD.D2Enums;
using DD.D2Structs;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
{
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
                                Game.Pickit.Disable(PickitDisableReason.InventoryFull);
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
}
