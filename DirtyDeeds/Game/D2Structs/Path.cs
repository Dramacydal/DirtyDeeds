using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Path
    {
        [FieldOffset(0x0)]
        public ushort xOffset;
        [FieldOffset(0x2)]
        public ushort xPos;
        [FieldOffset(0x4)]
        public ushort yOffset;
        [FieldOffset(0x6)]
        public ushort yPos;
        [FieldOffset(0x8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public ushort xTarget;
        [FieldOffset(0x12)]
        public ushort yTarget;
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x20)]
        public uint pRoomUnk;           // Room1*
        [FieldOffset(0x24)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x30)]
        public uint pUnit;              // UnitAny*
        [FieldOffset(0x34)]
        public uint dwFlags;
        [FieldOffset(0x38)]
        public uint _4;
        [FieldOffset(0x3C)]
        public uint dwPathType;
        [FieldOffset(0x40)]
        public uint dwPrevPathType;
        [FieldOffset(0x44)]
        public uint dwUnitSize;
        [FieldOffset(0x48)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x58)]
        public uint pTargetUnit;        // UnitAny*
        [FieldOffset(0x5C)]
        public uint dwTargetType;
        [FieldOffset(0x60)]
        public uint dwTargetId;
        [FieldOffset(0x64)]
        public byte bDirection;
    }
}
