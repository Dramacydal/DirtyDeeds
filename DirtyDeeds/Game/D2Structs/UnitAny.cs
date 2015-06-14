using DD.Game.Enums;
using System.Runtime.InteropServices;

namespace DD.Game.D2Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UnitAny
    {
        [FieldOffset(0x0)]
        public uint dwType;
        [FieldOffset(0x4)]
        public uint dwTxtFileNo;
        [FieldOffset(0x8)]
        public uint _1;
        [FieldOffset(0xC)]
        public uint dwUnitId;
        [FieldOffset(0x10)]
        public uint dwMode;
        [FieldOffset(0x14)]
        public uint pPlayerData;        // PlayerData*
        [FieldOffset(0x14)]
        public uint pItemData;          // ItemData*
        [FieldOffset(0x14)]
        public uint pMonsterData;       // MonsterData*
        [FieldOffset(0x14)]
        public uint pObjectData;        // ObjectData*
        [FieldOffset(0x18)]
        public uint dwAct;
        [FieldOffset(0x1C)]
        public uint pAct;               // Act*
        [FieldOffset(0x20)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] dwSeed;
        [FieldOffset(0x28)]
        public uint _2;
        [FieldOffset(0x2C)]
        public uint pPath;              // Path*
        [FieldOffset(0x2C)]
        public uint pItemPath;          // ItemPath*
        [FieldOffset(0x2C)]
        public uint pObjectPath;        // ObjectPath*
        [FieldOffset(0x30)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x44)]
        public uint dwGfxFrame;
        [FieldOffset(0x48)]
        public uint dwFrameRemain;
        [FieldOffset(0x4C)]
        public ushort wFrameRate;
        [FieldOffset(0x4E)]
        public ushort _4;
        [FieldOffset(0x50)]
        public uint pGfxUnk;            // BYTE*
        [FieldOffset(0x54)]
        public uint pGfxInfo;           // DWORD*
        [FieldOffset(0x58)]
        public uint _5;
        [FieldOffset(0x5C)]
        public uint pStats;             // StatList*
        [FieldOffset(0x60)]
        public uint pInventory;         // Inventory*
        [FieldOffset(0x64)]
        public uint ptLight;            // Light*
        [FieldOffset(0x68)]
        public uint dwStartLightRadius;
        [FieldOffset(0x6C)]
        public ushort nPl2ShiftIdx;
        [FieldOffset(0x6E)]
        public ushort nUpdateType;
        [FieldOffset(0x70)]
        public uint pUpdateUnit;        // UnitAny* - Used when updating unit.
        [FieldOffset(0x74)]
        public uint pQuestRecord;       // DWORD*
        [FieldOffset(0x78)]
        public uint bSparklyChest;      // bool
        [FieldOffset(0x7C)]
        public uint pTimerArgs;         // DWORD*
        [FieldOffset(0x80)]
        public uint dwSoundSync;
        [FieldOffset(0x84)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;
        [FieldOffset(0x8C)]
        public ushort wX;
        [FieldOffset(0x8E)]
        public ushort wY;
        [FieldOffset(0x90)]
        public uint _7;
        [FieldOffset(0x94)]
        public uint dwOwnerType;
        [FieldOffset(0x98)]
        public uint dwOwnerId;
        [FieldOffset(0x9C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _8;
        [FieldOffset(0xA4)]
        public uint pOMsg;              // OverheadMsg*
        [FieldOffset(0xA8)]
        public uint pInfo;              // Info*
        [FieldOffset(0xAC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _9;
        [FieldOffset(0xC4)]
        public uint dwFlags;
        [FieldOffset(0xC8)]
        public uint dwFlags2;
        [FieldOffset(0xCC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _10;
        [FieldOffset(0xE0)]
        public uint pChangedNext;       // UnitAny*
        [FieldOffset(0xE4)]
        public uint pListNext;          // UnitAny* 0xE4 -> 0xD8
        [FieldOffset(0xE8)]
        public uint pRoomNext;          // UnitAny*

        public bool IsRune()
        {
            if ((UnitType)dwType != UnitType.Item)
                return false;

            return dwTxtFileNo >= 610 && dwTxtFileNo <= 642;
        }

        public uint RuneNumber()
        {
            if (!IsRune())
                return 0;

            return dwTxtFileNo - 610 + 1;
        }
    }

}
