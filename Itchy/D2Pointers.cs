using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy
{
    public static class D2Common    // 0x6FD50000
    {
        public static uint GetLevel = 0x6D440;  // fastcall (ActMisc *pMisc, DWORD dwLevelNo)
        public static uint InitLevel = 0x6DDF0; // stdcall (Level *pLevel)
        //public static uint AutomapLayer2 = 0x30B00;//0x6FD9F0D0 - 0x6FD50000;
        public static uint GetLayer = 0x30B00;  // fastcall (DWORD dwLevelNo)
        public static uint AddRoomData = 0x24990;   // stdcall (Act * ptAct, int LevelId, int Xpos, int Ypos, Room1 * pRoom)
        public static uint RemoveRoomData = 0x24930;    // stdcall (Act* ptAct, int LevelId, int Xpos, int Ypos, Room1* pRoom)
        public static uint LoadAct = 0x24810;   // stdcall (DWORD ActNumber, DWORD MapId, DWORD Unk, DWORD Unk_2, DWORD Unk_3, DWORD Unk_4, DWORD TownLevelId, DWORD Func_1, DWORD Func_2)
        public static uint UnloadAct = 0x24590; // stdcall
        public static uint GetItemText = 0x62C70; // ItemTxt *__stdcall, (DWORD dwItemNo)
        public static uint pItemTextData = 0x6FDF4CB4 - 0x6FD50000;
        public static uint pMaxItemText = 0x6FDF4CB0 - 0x6FD50000;
        public static uint GetUnitStat = 0x584E0; // __stdcall, (UnitAny* pUnit, DWORD dwStat, DWORD dwStat2)
        public static uint GetItemPrice = 0x48620; // __stdcall, (UnitAny * player, UnitAny * item, DWORD difficulty, void* questinfo, int value, DWORD flag)
        public static uint sgptDataTables = 0x6FDF33F0 - 0x6FD50000;
        public static uint GetObjectTxt = 0x1ADC0; // ObjectTxt * __stdcall, (DWORD objno)
        public static uint GetFunkUnk_5 = 0x6FDBD1D0 - 0x6FD50000; // __stdcall, (DWORD nLevelNo)
        public static uint TestFunUnk_6 = 0x6FD67CF0 - 0x6FD50000; // DWORD __stdcall, (UnitAny *unit1, UnitAny *unit2, DWORD arg3)
    }

    public static class D2Client    // 0x6FAB0000
    {
        public static uint GetPlayerUnit = 0x613C0; // stdcall ()
        public static uint InitAutomapLayer_I = 0x733D0; // register (DWORD nLayerNo)
        public static uint pAutoMapLayer = 0x11CF28;
        public static uint RevealAutomapRoom = 0x73160; // stdcall (Room1 *pRoom1, DWORD dwClipFlag, AutomapLayer *aLayer)
        public static uint PrintGameString = 0x6FB25EB0 - 0x6FAB0000;
        public static uint pExpCharFlag = 0x1087B4;
        public static uint GetDifficulty = 0x42980; // cdecl
        public static uint pDifficulty = 0x6FBCD1D8 - 0x6FAB0000;
        public static uint LoadAct_1 = 0x737F0; // asm
        public static uint LoadAct_2 = 0x2B420; // asm
        public static uint pPlayerUnit = 0x11D050;
        public static uint ExitGame = 0x43870; // fastcall
        public static uint GetUiVar_I = 0x17C50; // register (DWORD dwVarNo)
        public static uint SetUiVar = 0x1C190; // __fastcall, (DWORD varno, DWORD howset, DWORD unknown1)
        public static uint pUiVars = 0x6FBCC890 - 0x6FAB0000;
        public static uint pServerUnitTable = 0x1047B8;
        public static uint pClientUnitTable = 0x103BB8;
        public static uint pPlayerUnitList = 0x11CB04;  // RosterUnit*
        public static uint pItemPriceList = 0x1018B3;  // __stdcall, (void)
        public static uint GetSelectedUnit = 0x17280; // __stdcall, (void)
        public static uint pMouseX = 0x11C950;  // DWORD
        public static uint pMouseY = 0x11C94C;  // DWORD
        public static uint NewAutomapCell = 0x703C0; // AutomapCell * __fastcall, (void)
        public static uint AddAutomapCell = 0x71EA0; // void __fastcall, (AutomapCell *aCell, AutomapCell **node)
        public static uint TestPvpFlag_I = 0x6A720;
    }

    public static class D2Net       // 0x6FBF0000
    {
        public static uint ReceivePacket = 0x6FBF64A0 - 0x6FBF0000;  // stdcall (BYTE *aPacket, DWORD aLen)
        public static uint SendPacket = 0x6F20; // stdcall (size_t aLen, DWORD arg1, BYTE* aPacket)
    }

    public static class Storm       // 0x6FBF0000
    {
        public static uint pHandle = 0x6FC42A50 - 0x6FBF0000;
    }

    public static class Fog         // 0x6FF50000
    {
        public static uint gdwBitMasks = 0x6FF77020 - 0x6FF50000;
    }
    
}
