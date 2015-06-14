using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ItemData
    {
        [FieldOffset(0)]
        public uint dwQuality;
        [FieldOffset(0x18)]
        public uint dwFlags;
        [FieldOffset(0x2C)]
        public uint dwItemLevel;
        [FieldOffset(0x44)]
        public uint BodyLocation;
        [FieldOffset(0x45)]
        public uint ItemLocation;
        [FieldOffset(0x69)]
        public byte NodePage;
        //[FieldOffset(0x70)]
        [FieldOffset(0x64)]
        public uint pNextInvItem;
        //[FieldOffset(0x104)]
        //public uint pOwner;
    }
}
