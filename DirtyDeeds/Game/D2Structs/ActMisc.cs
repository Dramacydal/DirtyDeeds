using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ActMisc
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x94)]
        public uint dwStaffTombLevel;
        [FieldOffset(0x98)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 245, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x46C)]
        public uint pAct;               // Act*
        [FieldOffset(0x470)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x47C)]
        public uint pLevelFirst;        // Level*
    }
}
