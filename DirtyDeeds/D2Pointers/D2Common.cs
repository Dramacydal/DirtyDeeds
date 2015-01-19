using WhiteMagic;
using WhiteMagic.Modules;

namespace DD.D2Pointers
{
    public class D2Common : ModulePointer    // 0x6FD50000
    {
        public D2Common(int offset)
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
}
