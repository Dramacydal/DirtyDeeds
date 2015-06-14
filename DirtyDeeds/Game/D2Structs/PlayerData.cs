using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    struct PlayerData
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        public string szName;
        [FieldOffset(0x10)]
        uint pNormalQuest;              // QuestInfo*
        [FieldOffset(0x14)]
        public uint pNightmareQuest;    // QuestInfo*
        [FieldOffset(0x18)]
        public uint pHellQuest;         // QuestInfo*
        [FieldOffset(0x1C)]
        public uint pNormalWaypoint;    // Waypoint*
        [FieldOffset(0x20)]
        public uint pNightmareWaypoint; // Waypoint*
        [FieldOffset(0x24)]
        public uint pHellWaypoint;      // Waypoint*
    }
}
