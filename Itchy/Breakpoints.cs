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
    class LightBreakPoint : HardwareBreakPoint
    {
        public LightBreakPoint(int address, uint len, Condition condition) : base(address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax = 0xFF;     // light density
            ctx.Eip += 0xEB;    // skip code

            return true;
        }
    }

    class RainBreakPoint : HardwareBreakPoint
    {
        public RainBreakPoint(int address, uint len, Condition condition) : base(address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax &= 0xFFFFFF00;
            ctx.Eip += 4;

            return true;
        }
    }

    // 6FB332FF
    class ReceivePacketBreakPoint : HardwareBreakPoint
    {
        public ReceivePacketBreakPoint(int address, uint len, Condition condition) : base(address, len, condition) { }

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
                    break;
                }
                case 0x15:
                {
                    var pPlayer = pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetPlayerUnit,
                        CallingConventionEx.StdCall);
                    var unit = pd.MemoryHandler.Read<UnitAny>(pPlayer);
                    
                    /*var pPlayer = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pPlayerUnit);
                    if (pPlayer == 0)
                        break;
                    var unit = pd.MemoryHandler.Read<UnitAny>(pPlayer);*/
                    if (BitConverter.ToUInt32(packet, 2) == unit.dwUnitId)
                    {
                        // no flash
                        pd.MemoryHandler.WriteByte(pPacket + 10, 0);

                        // fast tele
                        if (!allowableModes.Contains((PlayerMode)unit.dwMode))
                        {
                            unit.dwFrameRemain = 0;
                            pd.MemoryHandler.Write<UnitAny>(pPlayer, unit);
                        }
                    }
                    break;
                }
                case 0x51:
                {
                    // fast portal
                    if ((UnitType)packet[1] == UnitType.Object && BitConverter.ToUInt16(packet, 6) == 0x3B)
                        pd.MemoryHandler.WriteByte(pPacket + 12, 2);
                    break;
                }
                case 0xA7:
                {
                    // portal delay
                    if (packet[6] == 102)
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
