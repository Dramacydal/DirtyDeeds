using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CollMap
    {
        public uint dwPosGameX;         // 0x00
        public uint dwPosGameY;         // 0x04
        public uint dwSizeGameX;        // 0x08
        public uint dwSizeGameY;        // 0x0C
        public uint dwPosRoomX;         // 0x10
        public uint dwPosRoomY;         // 0x14
        public uint dwSizeRoomX;        // 0x18
        public uint dwSizeRoomY;        // 0x1C
        public uint pMapStart;          // 0x20 WORD*
        public uint pMapEnd;            // 0x24 WORD*
    }
}
