using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PresetUnit
    {
        public uint _1;                 // 0x00
        public uint dwTxtFileNo;        // 0x04
        public uint dwPosX;             // 0x08
        public uint pPresetNext;        // 0x0C PresetUnit
        public uint _3;                 // 0x10
        public uint dwType;             // 0x14
        public uint dwPosY;             // 0x18
    }
}
