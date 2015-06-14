using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Room2
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               //0x00
        [FieldOffset(0x8)]
        public uint pRoom2Near;         // Room2**
        [FieldOffset(0xC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x20)]
        public uint pType2Info;         // Type2Info*
        [FieldOffset(0x24)]
        public uint pRoom2Next;         // Room2*
        [FieldOffset(0x28)]
        public uint dwRoomFlags;
        [FieldOffset(0x2C)]
        public uint dwRoomsNear;
        [FieldOffset(0x30)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x34)]
        public uint dwPosX;
        [FieldOffset(0x38)]
        public uint dwPosY;
        [FieldOffset(0x3C)]
        public uint dwSizeX;
        [FieldOffset(0x40)]
        public uint dwSizeY;
        [FieldOffset(0x44)]
        public uint _3;
        [FieldOffset(0x48)]
        public uint dwPresetType;
        [FieldOffset(0x4C)]
        public uint pRoomTiles;         // RoomTile*
        [FieldOffset(0x50)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;
        [FieldOffset(0x58)]
        public uint pLevel;             // Level*
        [FieldOffset(0x5C)]
        public uint pPreset;            // PresetUnit*
    }
}
