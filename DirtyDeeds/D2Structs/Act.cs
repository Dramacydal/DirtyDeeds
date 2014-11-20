using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Act
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0xC)]
        public uint dwMapSeed;
        [FieldOffset(0x10)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x14)]
        public uint dwAct;
        [FieldOffset(0x18)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x48)]
        public uint pMisc;              // ActMisc*
    }
}
