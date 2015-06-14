using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Level
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2First;        // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint dwPosX;
        [FieldOffset(0x20)]
        public uint dwPosY;
        [FieldOffset(0x24)]
        public uint dwSizeX;
        [FieldOffset(0x28)]
        public uint dwSizeY;
        [FieldOffset(0x2C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x1AC)]
        public uint pNextLevel;         // Level*
        [FieldOffset(0x1B0)]
        public uint _4;
        [FieldOffset(0x1B4)]
        public uint pMisc;              // ActMisc*
        [FieldOffset(0x1BC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x1D0)]
        public uint dwLevelNo;
        [FieldOffset(0x1D4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;

        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterX;
        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpX;

        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterY;
        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpY;

        [FieldOffset(0x228)]
        public uint dwRoomEntries;

        public bool IsTown() { return dwLevelNo == 1 || dwLevelNo == 40 || dwLevelNo == 75 || dwLevelNo == 103 || dwLevelNo == 109; }
        public bool IsUberTristram() { return dwLevelNo == 136; }
    }
}
