using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DD.D2Structs
{
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
}
