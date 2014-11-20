using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Inventory
    {
        [FieldOffset(0x0)]
        public uint dwSignature;
        [FieldOffset(0x04)]
        public uint bGame1C;            // BYTE*
        [FieldOffset(0x08)]
        public uint pOwner;             // UnitAny*
        [FieldOffset(0x0C)]
        public uint pFirstItem;         // UnitAny*
        [FieldOffset(0x10)]
        public uint pLastItem;          // UnitAny*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x1C)]
        public uint dwLeftItemUid;
        [FieldOffset(0x20)]
        public uint pCursorItem;        // UnitAny*
        [FieldOffset(0x24)]
        public uint dwOwnerId;
        [FieldOffset(0x28)]
        public uint dwItemCount;
    }
}
