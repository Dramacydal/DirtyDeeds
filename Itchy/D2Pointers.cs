using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public class D2CommonPointer : ModulePointer
    {
        public D2CommonPointer(uint offset)
            : base("d2common.dll", offset)
        { }
    }

    public class D2ClientPointer : ModulePointer
    {
        public D2ClientPointer(uint offset)
            : base("d2client.dll", offset)
        { }
    }

    public class D2NetPointer : ModulePointer
    {
        public D2NetPointer(uint offset)
            : base("d2net.dll", offset)
        { }
    }

    public class StormPointer : ModulePointer
    {
        public StormPointer(uint offset)
            : base("storm.dll", offset)
        { }
    }

    public class FogPointer : ModulePointer
    {
        public FogPointer(uint offset)
            : base("fog.dll", offset)
        { }
    }

    public static class D2Common    // 0x6FD50000
    {
        public static D2CommonPointer GetLevel = new D2CommonPointer(0x6D440);  // fastcall (ActMisc *pMisc, DWORD dwLevelNo)
        public static D2CommonPointer InitLevel = new D2CommonPointer(0x6DDF0); // stdcall (Level *pLevel)
        //public static D2CommonOffset AutomapLayer2 = new D2CommonOffset(0x30B00;//0x6FD9F0D0 - 0x6FD50000);
        public static D2CommonPointer GetLayer = new D2CommonPointer(0x30B00);  // fastcall (DWORD dwLevelNo)
        public static D2CommonPointer AddRoomData = new D2CommonPointer(0x24990);   // stdcall (Act * ptAct, int LevelId, int Xpos, int Ypos, Room1 * pRoom)
        public static D2CommonPointer RemoveRoomData = new D2CommonPointer(0x24930);    // stdcall (Act* ptAct, int LevelId, int Xpos, int Ypos, Room1* pRoom)
        public static D2CommonPointer LoadAct = new D2CommonPointer(0x24810);   // stdcall (DWORD ActNumber, DWORD MapId, DWORD Unk, DWORD Unk_2, DWORD Unk_3, DWORD Unk_4, DWORD TownLevelId, DWORD Func_1, DWORD Func_2)
        public static D2CommonPointer UnloadAct = new D2CommonPointer(0x24590); // stdcall
        public static D2CommonPointer GetItemText = new D2CommonPointer(0x62C70); // ItemTxt *__stdcall, (DWORD dwItemNo)
        public static D2CommonPointer pItemTextData = new D2CommonPointer(0x6FDF4CB4 - 0x6FD50000);
        public static D2CommonPointer pMaxItemText = new D2CommonPointer(0x6FDF4CB0 - 0x6FD50000);
        public static D2CommonPointer GetUnitStat = new D2CommonPointer(0x584E0); // __stdcall, (UnitAny* pUnit, DWORD dwStat, DWORD dwStat2)
        public static D2CommonPointer GetItemPrice = new D2CommonPointer(0x48620); // __stdcall, (UnitAny * player, UnitAny * item, DWORD difficulty, void* questinfo, int value, DWORD flag)
        public static D2CommonPointer sgptDataTables = new D2CommonPointer(0x6FDF33F0 - 0x6FD50000);
        public static D2CommonPointer GetObjectTxt = new D2CommonPointer(0x1ADC0); // ObjectTxt * __stdcall, (DWORD objno)
        public static D2CommonPointer GetFunkUnk_5 = new D2CommonPointer(0x6FDBD1D0 - 0x6FD50000); // __stdcall, (DWORD nLevelNo)
        public static D2CommonPointer TestFunUnk_6 = new D2CommonPointer(0x6FD67CF0 - 0x6FD50000); // DWORD __stdcall, (UnitAny *unit1, UnitAny *unit2, DWORD arg3)
    }

    public static class D2Client    // 0x6FAB0000
    {
        public static D2ClientPointer GetPlayerUnit = new D2ClientPointer(0x613C0); // stdcall ()
        public static D2ClientPointer InitAutomapLayer_I = new D2ClientPointer(0x733D0); // register (DWORD nLayerNo)
        public static D2ClientPointer pAutoMapLayer = new D2ClientPointer(0x11CF28);
        public static D2ClientPointer RevealAutomapRoom = new D2ClientPointer(0x73160); // stdcall (Room1 *pRoom1, DWORD dwClipFlag, AutomapLayer *aLayer)
        public static D2ClientPointer PrintGameString = new D2ClientPointer(0x6FB25EB0 - 0x6FAB0000);
        public static D2ClientPointer pExpCharFlag = new D2ClientPointer(0x1087B4);
        public static D2ClientPointer GetDifficulty = new D2ClientPointer(0x42980); // cdecl
        public static D2ClientPointer pDifficulty = new D2ClientPointer(0x6FBCD1D8 - 0x6FAB0000);
        public static D2ClientPointer LoadAct_1 = new D2ClientPointer(0x737F0); // asm
        public static D2ClientPointer LoadAct_2 = new D2ClientPointer(0x2B420); // asm
        public static D2ClientPointer pPlayerUnit = new D2ClientPointer(0x11D050);
        public static D2ClientPointer ExitGame = new D2ClientPointer(0x43870); // fastcall
        public static D2ClientPointer GetUiVar_I = new D2ClientPointer(0x17C50); // register (DWORD dwVarNo)
        public static D2ClientPointer SetUiVar = new D2ClientPointer(0x1C190); // __fastcall, (DWORD varno, DWORD howset, DWORD unknown1)
        public static D2ClientPointer pUiVars = new D2ClientPointer(0x6FBCC890 - 0x6FAB0000);
        public static D2ClientPointer pServerUnitTable = new D2ClientPointer(0x1047B8);
        public static D2ClientPointer pClientUnitTable = new D2ClientPointer(0x103BB8);
        public static D2ClientPointer pPlayerUnitList = new D2ClientPointer(0x11CB04);  // RosterUnit*
        public static D2ClientPointer pItemPriceList = new D2ClientPointer(0x1018B3);  // __stdcall, (void)
        public static D2ClientPointer GetSelectedUnit = new D2ClientPointer(0x17280); // __stdcall, (void)
        public static D2ClientPointer pMouseX = new D2ClientPointer(0x11C950);  // DWORD
        public static D2ClientPointer pMouseY = new D2ClientPointer(0x11C94C);  // DWORD
        public static D2ClientPointer NewAutomapCell = new D2ClientPointer(0x703C0); // AutomapCell * __fastcall, (void)
        public static D2ClientPointer AddAutomapCell = new D2ClientPointer(0x71EA0); // void __fastcall, (AutomapCell *aCell, AutomapCell **node)
        public static D2ClientPointer TestPvpFlag_I = new D2ClientPointer(0x6A720);
    }

    public static class D2Net       // 0x6FBF0000
    {
        public static D2NetPointer ReceivePacket = new D2NetPointer(0x6FBF64A0 - 0x6FBF0000);  // stdcall (BYTE *aPacket, DWORD aLen)
        public static D2NetPointer SendPacket = new D2NetPointer(0x6F20); // stdcall (size_t aLen, DWORD arg1, BYTE* aPacket)
    }

    public static class Storm       // 0x6FBF0000
    {
        public static StormPointer pHandle = new StormPointer(0x6FC42A50 - 0x6FBF0000);
    }

    public static class Fog         // 0x6FF50000
    {
        public static FogPointer gdwBitMasks = new FogPointer(0x6FF77020 - 0x6FF50000);
    }
    
}
