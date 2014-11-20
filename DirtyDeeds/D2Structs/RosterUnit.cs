using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RosterUnit
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        public string szName;           // 0x00
        public uint dwUnitId;           // 0x10
        public uint dwPartyLife;        // 0x14
        public uint _1;                 // 0x18
        public uint dwClassId;          // 0x1C
        public ushort wLevel;           // 0x20
        public ushort wPartyId;         // 0x22
        public uint dwLevelId;          // 0x24
        public uint Xpos;               // 0x28
        public uint Ypos;               // 0x2C
        public uint dwPartyFlags;       // 0x30
        public uint _5;                 // 0x34 BYTE*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;               // 0x38
        public ushort _7;               // 0x64
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        public string szName2;          // 0x66
        public ushort _8;               // 0x76
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _9;               // 0x78
        public uint pNext;              // 0x80 RosterUnit*
    }
}
