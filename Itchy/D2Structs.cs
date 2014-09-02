using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Itchy.D2Enums;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Info
    {
        public uint pGame1C;            // 0x00 BYTE*
        public uint pFirstSkill;        // 0x04 Skill*
        public uint pLeftSkill;         // 0x08 Skill*
        public uint pRightSkill;        // 0x0C Skill*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Inventory
    {
        [FieldOffset(0x0)]
        public uint dwSignature;
        [FieldOffset(0x04)]
        public uint bGame1C;            // BYTE*
        [FieldOffset(0x08)]
        public uint pOwner;             // UnitAny*
        [FieldOffset(0x0C)]
        public uint pFirstItem;         // UnitAny*
        [FieldOffset(0x10)]
        public uint pLastItem;          // UnitAny*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x1C)]
        public uint dwLeftItemUid;
        [FieldOffset(0x20)]
        public uint pCursorItem;        // UnitAny*
        [FieldOffset(0x24)]
        public uint dwOwnerId;
        [FieldOffset(0x28)]
        public uint dwItemCount;
    }

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


    [StructLayout(LayoutKind.Explicit)]
    public struct ItemTxt
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szFlippyFile;     // 0x00
        [FieldOffset(0x20)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szInvFile;        // 0x20
        [FieldOffset(0x40)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szUniqueInvFile;  // 0x40
        [FieldOffset(0x60)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szSetInvFile;     // 0x60
        [FieldOffset(0x80)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] szCode;           // 0x40
        [FieldOffset(0xFC)]
        public byte rarity;
        [FieldOffset(0x10F)]
        public byte xSize;              // 0x10F
        [FieldOffset(0x110)]
        public byte ySize;              // 0x110

        public string GetCode()
        {
            return Encoding.ASCII.GetString(szCode).Replace(" ", "");
        }

        public uint GetDwCode()
        {
            return BitConverter.ToUInt32(szCode, 0) & 0xFFF;
        }
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

        public bool IsTown() { return dwLevelNo == 1 || dwLevelNo == 40 || dwLevelNo == 75 || dwLevelNo == 103 || dwLevelNo == 109; }
        public bool IsUberTristram() { return dwLevelNo == 136; }
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RoomTile
    {
        public uint pRoom2;             // 0x00 Room2*
        public uint pNext;              // 0x04 RoomTile*
        public uint _1;                 // 0x08
        public uint _2;                 // 0x08
        public uint nNum;               // 0x10 DWORD *
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LevelTxt
    {
        public uint dwLevelNo;          //0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               //0x04
        public byte _2;                 //0xF4
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string szName;           //+16e
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string szEntranceText;   //0x11D
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string szLevelDesc;      //0x145
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

    public struct ObjectPath
    {
        public uint pRoom1;             // 0x00 Room1*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               // 0x04
        public uint dwPosX;             // 0x0C
        public uint dwPosY;             // 0x10
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RosterUnit
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        public string szName;           // 0x00
        public uint dwUnitId;           // 0x10
        public uint dwPartyLife;        // 0x14
        public uint _1;                 // 0x18
        public uint dwClassId;          // 0x1C
        public ushort wLevel;           // 0x20
        public ushort wPartyId;         // 0x22
        public uint dwLevelId;          // 0x24
        public uint Xpos;               // 0x28
        public uint Ypos;               // 0x2C
        public uint dwPartyFlags;       // 0x30
        public uint _5;                 // 0x34 BYTE*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;               // 0x38
        public ushort _7;               // 0x64
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        public string szName2;          // 0x66
        public ushort _8;               // 0x76
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _9;               // 0x78
        public uint pNext;              // 0x80 RosterUnit*
    }

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectTxt
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.U1)]
        public byte[] szName;           // 0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.U1)]
        public byte[] wszName;          // 0x40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] _1;               // 0xC0
        public byte nSelectable0;       // 0xC4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x87, ArraySubType = UnmanagedType.U1)]
        public byte[] _2;               // 0xC5
        public byte nOrientation;       // 0x14C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19, ArraySubType = UnmanagedType.U1)]
        public byte[] _2b;              // 0x14D
        public byte nSubClass;          // 0x166
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x11, ArraySubType = UnmanagedType.U1)]
        public byte[] _3;               // 0x167
        public byte nParm0;             // 0x178
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x39, ArraySubType = UnmanagedType.U1)]
        public byte[] _4;               // 0x179
        public byte nPopulateFn;        // 0x1B2
        public byte nOperateFn;         // 0x1B3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] _5;               // 0x1B4
        public uint nAutoMap;           // 0x1BB
    }

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

    public struct Skill
    {
        public uint pSkillInfo;         // 0x00 SkillInfo*
        public uint pNextSkill;         // 0x04 Skill*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               // 0x08
        public uint dwSkillLevel;       // 0x28
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;               // 0x2C
        public uint ItemId;             // 0x34 0xFFFFFFFF if not a charge
        public uint ChargesLeft;        // 0x38 
        public uint IsCharge;           // 0x3C 1 for charge, else 0
    }

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct SkillInfo
    {
        public ushort wSkillId;         // 0x00
    }

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
