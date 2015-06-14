using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RoomTile
    {
        public uint pRoom2;             // 0x00 Room2*
        public uint pNext;              // 0x04 RoomTile*
        public uint _1;                 // 0x08
        public uint _2;                 // 0x08
        public uint nNum;               // 0x10 DWORD *
    }
}
