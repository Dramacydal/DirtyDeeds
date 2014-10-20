using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public class D2Common : ModulePointer    // 0x6FD50000
    {
        public D2Common(uint offset)
            : base("D2Common.dll", offset)
        { }

        public static D2Common GetLevel = new D2Common(0x6D440);  // fastcall (ActMisc *pMisc, DWORD dwLevelNo)
        public static D2Common InitLevel = new D2Common(0x6DDF0); // stdcall (Level *pLevel)
        //public static D2CommonOffset AutomapLayer2 = new D2CommonOffset(0x30B00;//0x6FD9F0D0 - 0x6FD50000);
        public static D2Common GetLayer = new D2Common(0x30B00);  // fastcall (DWORD dwLevelNo)
        public static D2Common AddRoomData = new D2Common(0x24990);   // stdcall (Act * ptAct, int LevelId, int Xpos, int Ypos, Room1 * pRoom)
        public static D2Common RemoveRoomData = new D2Common(0x24930);    // stdcall (Act* ptAct, int LevelId, int Xpos, int Ypos, Room1* pRoom)
        public static D2Common LoadAct = new D2Common(0x24810);   // stdcall (DWORD ActNumber, DWORD MapId, DWORD Unk, DWORD Unk_2, DWORD Unk_3, DWORD Unk_4, DWORD TownLevelId, DWORD Func_1, DWORD Func_2)
        public static D2Common UnloadAct = new D2Common(0x24590); // stdcall
        public static D2Common GetItemText = new D2Common(0x62C70); // ItemTxt *__stdcall, (DWORD dwItemNo)
        public static D2Common pItemTextData = new D2Common(0x6FDF4CB4 - 0x6FD50000);
        public static D2Common pMaxItemText = new D2Common(0x6FDF4CB0 - 0x6FD50000);
        public static D2Common GetUnitStat = new D2Common(0x584E0); // __stdcall, (UnitAny* pUnit, DWORD dwStat, DWORD dwStat2)
        public static D2Common GetBaseUnitStat = new D2Common(0x6FDA8590 - 0x6FD50000); // DWORD __stdcall, (UnitAny *Unit, DWORD dwStat, DWORD dwUkn)
        public static D2Common GetItemPrice = new D2Common(0x48620); // __stdcall, (UnitAny * player, UnitAny * item, DWORD difficulty, void* questinfo, int value, DWORD flag)
        public static D2Common sgptDataTables = new D2Common(0x6FDF33F0 - 0x6FD50000);
        public static D2Common GetObjectTxt = new D2Common(0x1ADC0); // ObjectTxt * __stdcall, (DWORD objno)
        public static D2Common GetFunkUnk_5 = new D2Common(0x6FDBD1D0 - 0x6FD50000); // __stdcall, (DWORD nLevelNo)
        public static D2Common TestFunUnk_6 = new D2Common(0x6FD67CF0 - 0x6FD50000); // DWORD __stdcall, (UnitAny *unit1, UnitAny *unit2, DWORD arg3)
        public static D2Common GetLevelText = new D2Common(0x30CA0); // LevelTxt * __stdcall, (DWORD levelno)
    }

    public class D2Client : ModulePointer    // 0x6FAB0000
    {
        public D2Client(uint offset)
            : base("D2Client.dll", offset)
        { }

        public static D2Client GetPlayerUnit = new D2Client(0x613C0); // stdcall ()
        public static D2Client InitAutomapLayer_I = new D2Client(0x733D0); // register (DWORD nLayerNo)
        public static D2Client pAutoMapLayer = new D2Client(0x11CF28);
        public static D2Client RevealAutomapRoom = new D2Client(0x73160); // stdcall (Room1 *pRoom1, DWORD dwClipFlag, AutomapLayer *aLayer)
        public static D2Client PrintGameString = new D2Client(0x6FB25EB0 - 0x6FAB0000);
        public static D2Client pExpCharFlag = new D2Client(0x1087B4);
        public static D2Client GetDifficulty = new D2Client(0x42980); // cdecl
        public static D2Client pDifficulty = new D2Client(0x6FBCD1D8 - 0x6FAB0000);
        public static D2Client LoadAct_1 = new D2Client(0x737F0); // asm
        public static D2Client LoadAct_2 = new D2Client(0x2B420); // asm
        public static D2Client pPlayerUnit = new D2Client(0x11D050);
        public static D2Client ExitGame = new D2Client(0x43870); // fastcall
        public static D2Client GetUiVar_I = new D2Client(0x17C50); // register (DWORD dwVarNo)
        public static D2Client SetUiVar = new D2Client(0x1C190); // __fastcall, (DWORD varno, DWORD howset, DWORD unknown1)
        public static D2Client pUiVars = new D2Client(0x6FBCC890 - 0x6FAB0000);
        public static D2Client pServerUnitTable = new D2Client(0x1047B8);
        public static D2Client pClientUnitTable = new D2Client(0x103BB8);
        public static D2Client pPlayerUnitList = new D2Client(0x11CB04);  // RosterUnit*
        public static D2Client pItemPriceList = new D2Client(0x1018B3);  // __stdcall, (void)
        public static D2Client GetSelectedUnit = new D2Client(0x17280); // __stdcall, (void)
        public static D2Client pMouseX = new D2Client(0x11C950);  // DWORD
        public static D2Client pMouseY = new D2Client(0x11C94C);  // DWORD
        public static D2Client NewAutomapCell = new D2Client(0x703C0); // AutomapCell * __fastcall, (void)
        public static D2Client AddAutomapCell = new D2Client(0x71EA0); // void __fastcall, (AutomapCell *aCell, AutomapCell **node)
        public static D2Client TestPvpFlag_I = new D2Client(0x6A720);
        public static D2Client GetUnitX = new D2Client(0x1210); // int __fastcall, (UnitAny* pUnit)
        public static D2Client GetUnitY = new D2Client(0x1240); // int __fastcall, (UnitAny* pUnit)
    }

    public class D2Net : ModulePointer   // 0x6FBF0000
    {
        public D2Net(uint offset)
            : base("D2Net.dll", offset)
        { }

        public static D2Net ReceivePacket = new D2Net(0x6FBF64A0 - 0x6FBF0000);  // stdcall (BYTE *aPacket, DWORD aLen)
        public static D2Net SendPacket = new D2Net(0x6F20); // stdcall (size_t aLen, DWORD arg1, BYTE* aPacket)
    }

    public class Storm : ModulePointer   // 0x6FBF0000
    {
        public Storm(uint offset)
            : base("Storm.dll", offset)
        { }

        public static Storm pHandle = new Storm(0x6FC42A50 - 0x6FBF0000);
    }

    public class Fog : ModulePointer     // 0x6FF50000
    {
        public Fog(uint offset)
            : base("Fog.dll", offset)
        { }

        public static Fog gdwBitMasks = new Fog(0x6FF77020 - 0x6FF50000);
    }

    public class Game : ModulePointer   // 0x400000
    {
        public Game(uint offset)
            : base("Game.exe", offset)
        { }

        public static Game mainThread = new Game(0x401227 - 0x400000);
    }
}
