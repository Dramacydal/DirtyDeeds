using System.Runtime.InteropServices;

namespace DD.D2Structs
{
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
}
