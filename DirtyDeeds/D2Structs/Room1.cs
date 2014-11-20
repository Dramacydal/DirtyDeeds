using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Room1
    {
        [FieldOffset(0x0)]
        public uint pRoomsNear;         // Room1**
        [FieldOffset(0x04)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2;             // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x20)]
        public uint Coll;               // CollMap*
        [FieldOffset(0x24)]
        public uint dwRoomsNear;
        [FieldOffset(0x28)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x4C)]
        public uint dwXStart;
        [FieldOffset(0x50)]
        public uint dwYStart;
        [FieldOffset(0x54)]
        public uint dwXSize;
        [FieldOffset(0x58)]
        public uint dwYSize;
        [FieldOffset(0x5C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;
        [FieldOffset(0x74)]
        public uint pUnitFirst;         // UnitAny*
        [FieldOffset(0x78)]
        public uint _5;
        [FieldOffset(0x7C)]
        public uint pRoomNext;          // Room1*
    }
}
