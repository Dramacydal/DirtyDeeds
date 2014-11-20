using WhiteMagic;

namespace DD.D2Pointers
{
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
}
