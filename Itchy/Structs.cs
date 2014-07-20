using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Itchy
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
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Act
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0xC)]
        public uint dwMapSeed;
        [FieldOffset(0x10)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x14)]
        public uint dwAct;
        [FieldOffset(0x18)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x48)]
        public uint pMisc;              // ActMisc*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ActMisc
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x94)]
        public uint dwStaffTombLevel;
        [FieldOffset(0x98)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 245, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x46C)]
        public uint pAct;               // Act*
        [FieldOffset(0x470)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x47C)]
        public uint pLevelFirst;        // Level*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Level
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2First;        // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint dwPosX;
        [FieldOffset(0x20)]
        public uint dwPosY;
        [FieldOffset(0x24)]
        public uint dwSizeX;
        [FieldOffset(0x28)]
        public uint dwSizeY;
        [FieldOffset(0x2C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x1AC)]
        public uint pNextLevel;         // Level*
        [FieldOffset(0x1B0)]
        public uint _4;
        [FieldOffset(0x1B4)]
        public uint pMisc;              // ActMisc*
        [FieldOffset(0x1BC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x1D0)]
        public uint dwLevelNo;
        [FieldOffset(0x1D4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;


        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterX;
        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpX;

        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterY;
        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpY;

        [FieldOffset(0x228)]
        public uint dwRoomEntries;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Room1
    {
        [FieldOffset(0x0)]
        public uint pRoomsNear;         // Room1**
        [FieldOffset(0x04)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2;             // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x20)]
        public uint Coll;               // CollMap*
        [FieldOffset(0x24)]
        public uint dwRoomsNear;
        [FieldOffset(0x28)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x4C)]
        public uint dwXStart;
        [FieldOffset(0x50)]
        public uint dwYStart;
        [FieldOffset(0x54)]
        public uint dwXSize;
        [FieldOffset(0x58)]
        public uint dwYSize;
        [FieldOffset(0x5C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;
        [FieldOffset(0x74)]
        public uint pUnitFirst;         // UnitAny*
        [FieldOffset(0x78)]
        public uint _5;
        [FieldOffset(0x7C)]
        public uint pRoomNext;          // Room1*
    }

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

    [StructLayout(LayoutKind.Explicit)]
    public struct Type2Info
    {
        [FieldOffset(0x0)]
        public uint dwRoomNumber;
        [FieldOffset(0x4)]
        public uint _1;
        [FieldOffset(0x8)]
        public uint pdwSubNumber;       // DWORD*
    }

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

    [StructLayout(LayoutKind.Explicit)]
    public struct AutomapLayer2
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x08)]
        public uint nLayerNo;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Path
    {
        [FieldOffset(0x0)]
        public ushort xOffset;
        [FieldOffset(0x2)]
        public ushort xPos;
        [FieldOffset(0x4)]
        public ushort yOffset;
        [FieldOffset(0x6)]
        public ushort yPos;
        [FieldOffset(0x8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public ushort xTarget;
        [FieldOffset(0x12)]
        public ushort yTarget;
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x20)]
        public uint pRoomUnk;           // Room1*
        [FieldOffset(0x24)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x30)]
        public uint pUnit;              // UnitAny*
        [FieldOffset(0x34)]
        public uint dwFlags;
        [FieldOffset(0x38)]
        public uint _4;
        [FieldOffset(0x3C)]
        public uint dwPathType;
        [FieldOffset(0x40)]
        public uint dwPrevPathType;
        [FieldOffset(0x44)]
        public uint dwUnitSize;
        [FieldOffset(0x48)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x58)]
        public uint pTargetUnit;        // UnitAny*
        [FieldOffset(0x5C)]
        public uint dwTargetType;
        [FieldOffset(0x60)]
        public uint dwTargetId;
        [FieldOffset(0x64)]
        public byte bDirection;
    }
}
