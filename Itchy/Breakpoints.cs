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
            ctx.Eax &= 0xFFFFFF00;
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

            var packet = pd.MemoryHandler.ReadBytes(pPacket, (int)len);

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
                        var unit = pd.MemoryHandler.Read<UnitAny>(pUnit);
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
                    
                    var pPlayer = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pPlayerUnit);
                    if (pPlayer == 0)
                        break;

                    var unit = pd.MemoryHandler.Read<UnitAny>(pPlayer);
                    if (BitConverter.ToUInt32(packet, 2) == unit.dwUnitId)
                    {
                        // no flash
                        if (Game.Settings.ReceivePacketHack.BlockFlash)
                            pd.MemoryHandler.WriteByte(pPacket + 10, 0);

                        // fast teleport
                        if (Game.Settings.ReceivePacketHack.FastTele && !allowableModes.Contains((PlayerMode)unit.dwMode))
                        {
                            unit.dwFrameRemain = 0;
                            pd.MemoryHandler.Write<UnitAny>(pPlayer, unit);
                        }
                    }
                    break;
                }
                case 0x18:
                case 0x95:
                {
                    var life = BitConverter.ToUInt16(packet, 1);
                    var mana = BitConverter.ToUInt16(packet, 3);
                    if (Game.Settings.Chicken.Enabled && !Game.chickening)
                        Task.Factory.StartNew(() => Game.Chickener(false));
                    break;
                }
                case 0x51:
                {
                    if ((UnitType)packet[1] == UnitType.Object && BitConverter.ToUInt16(packet, 6) == 0x3B)
                    {
                        // no portal anim #2
                        if (Game.Settings.ReceivePacketHack.FastPortal)
                            pd.MemoryHandler.WriteByte(pPacket + 12, 2);

                        UnitAny unit;
                        if (Game.backToTown && Game.GetPlayerUnit(out unit))
                        {
                            if (!Game.IsInTown())
                            {
                                var path = pd.MemoryHandler.Read<Path>(unit.pPath);
                                if (Misc.Distance(path.xPos, path.yPos, BitConverter.ToUInt16(packet, 8), BitConverter.ToUInt16(packet, 10)) < 10)
                                    Task.Factory.StartNew(() => Game.Interact(BitConverter.ToUInt32(packet, 2), UnitType.Object));
                            }
                            Game.backToTown = false;
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
}
