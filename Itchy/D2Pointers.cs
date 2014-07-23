using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy
{
    public static class D2Common
    {
        public static uint GetLevel = 0x6D440;  // fastcall (ActMisc *pMisc, DWORD dwLevelNo)
        public static uint InitLevel = 0x6DDF0; // stdcall (Level *pLevel)
        //public static uint AutomapLayer2 = 0x30B00;//0x6FD9F0D0 - 0x6FD50000;
        public static uint GetLayer = 0x30B00;  // fastcall (DWORD dwLevelNo)
        public static uint AddRoomData = 0x24990;   // stdcall (Act * ptAct, int LevelId, int Xpos, int Ypos, Room1 * pRoom)
        public static uint RemoveRoomData = 0x24930;    // stdcall (Act* ptAct, int LevelId, int Xpos, int Ypos, Room1* pRoom)
        public static uint LoadAct = 0x24810;   // stdcall (DWORD ActNumber, DWORD MapId, DWORD Unk, DWORD Unk_2, DWORD Unk_3, DWORD Unk_4, DWORD TownLevelId, DWORD Func_1, DWORD Func_2)
        public static uint UnloadAct = 0x24590; // stdcall
    }

    public static class D2Client
    {
        public static uint GetPlayerUnit = 0x613C0; // stdcall ()
        public static uint InitAutomapLayer_I = 0x733D0; // register (DWORD nLayerNo)
        public static uint pAutoMapLayer = 0x11CF28;
        public static uint RevealAutomapRoom = 0x73160; // stdcall (Room1 *pRoom1, DWORD dwClipFlag, AutomapLayer *aLayer)
        public static uint PrintGameString = 0x6FB25EB0 - 0x6FAB0000;
        public static uint pExpCharFlag = 0x1087B4;
        public static uint GetDifficulty = 0x42980; // stdcall
        public static uint LoadAct_1 = 0x737F0; // asm
        public static uint LoadAct_2 = 0x2B420; // asm
        public static uint pPlayerUnit = 0x11D050;
    }

    public static class D2Net
    {
        public static uint ReceivePacket = 0x6FBF64A0 - 0x6FBF0000;  // stdcall (BYTE *aPacket, DWORD aLen)
        public static uint SendPacket = 0x6F20; // stdcall (size_t aLen, DWORD arg1, BYTE* aPacket)

    }
}
