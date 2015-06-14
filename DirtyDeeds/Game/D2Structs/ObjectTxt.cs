using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectTxt
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
        public string szName;           // 0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.U1)]
        public byte[] wszName;          // 0x40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] _1;               // 0xC0
        public byte nSelectable0;       // 0xC4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x87, ArraySubType = UnmanagedType.U1)]
        public byte[] _2;               // 0xC5
        public byte nOrientation;       // 0x14C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19, ArraySubType = UnmanagedType.U1)]
        public byte[] _2b;              // 0x14D
        public byte nSubClass;          // 0x166
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x11, ArraySubType = UnmanagedType.U1)]
        public byte[] _3;               // 0x167
        public byte nParm0;             // 0x178
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x39, ArraySubType = UnmanagedType.U1)]
        public byte[] _4;               // 0x179
        public byte nPopulateFn;        // 0x1B2
        public byte nOperateFn;         // 0x1B3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] _5;               // 0x1B4
        public uint nAutoMap;           // 0x1BB
    }
}
