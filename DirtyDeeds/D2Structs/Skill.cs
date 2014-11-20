using System.Runtime.InteropServices;

namespace DD.D2Structs
{
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
}
