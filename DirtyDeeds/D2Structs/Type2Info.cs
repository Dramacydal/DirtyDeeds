using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Type2Info
    {
        [FieldOffset(0x0)]
        public uint dwRoomNumber;
        [FieldOffset(0x4)]
        public uint _1;
        [FieldOffset(0x8)]
        public uint pdwSubNumber;       // DWORD*
    }
}
