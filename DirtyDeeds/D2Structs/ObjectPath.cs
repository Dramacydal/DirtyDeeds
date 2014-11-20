using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectPath
    {
        public uint pRoom1;             // 0x00 Room1*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               // 0x04
        public uint dwPosX;             // 0x0C
        public uint dwPosY;             // 0x10
    }
}
