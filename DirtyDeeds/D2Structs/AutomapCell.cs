using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AutomapCell
    {
        public uint fSaved;             // 0x00
        public ushort nCellNo;          // 0x04
        public ushort xPixel;           // 0x06
        public ushort yPixel;           // 0x08
        public ushort wWeight;          // 0x0A
        public uint pLess;              // 0x0C AutomapCell*
        public uint pMore;              // 0x10 AutomapCell*
    }
}
