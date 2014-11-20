using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Info
    {
        public uint pGame1C;            // 0x00 BYTE*
        public uint pFirstSkill;        // 0x04 Skill*
        public uint pLeftSkill;         // 0x08 Skill*
        public uint pRightSkill;        // 0x0C Skill*
    }
}
