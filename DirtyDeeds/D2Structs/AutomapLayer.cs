using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AutomapLayer
    {
        [FieldOffset(0x0)]
        public uint nLayerNo;
        [FieldOffset(0x04)]
        public uint fSaved;
        [FieldOffset(0x08)]
        public uint pFloors;            // AutomapCell*
        [FieldOffset(0x0C)]
        public uint pWalls;             // AutomapCell*
        [FieldOffset(0x10)]
        public uint pObjects;           // AutomapCell*
        [FieldOffset(0x14)]
        public uint pExtras;            // AutomapCell*
        [FieldOffset(0x18)]
        public uint pNextLayer;         // AutomapLayer*
    };
}
