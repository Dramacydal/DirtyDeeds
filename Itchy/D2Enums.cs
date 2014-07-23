using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy
{
    public enum PlayerMode : uint
    {
        Death = 0,      // death
        Stand_OutTown = 1,  // standing outside town
        Walk_OutTown = 2,   // walking outside town
        Run = 3,            // running
        Being_Hit = 4,      // being hit
        Stand_InTown = 5,   // standing inside town
        Walk_InTown = 6,    // walking outside town
        Attack1 = 7,        // attacking 1
        Attack2 = 8,        // attacking 2
        Block = 9,          // blocking
        Cast = 10,           // casting spell
        Throw = 11,          // throwing
        Kick = 12,           // kicking (assassin)
        UseSkill1 = 13,      // using skill 1
        UseSkill2 = 14,      // using skill 2
        UseSkill3 = 15,      // using skill 3
        UseSkill4 = 16,      // using skill 4
        Dead = 17,           // dead
        Sequence = 18,       // sequence
        Being_Knockback = 19 // being knocked back
    }

    public enum UnitType : uint
    {
        Player = 0,
        Npc = 1,
        Monster = 1,
        Object = 2,
        Missile = 3,
        Item = 4,
        Tile = 5
    }

    public enum D2Color : uint
    {
        Default = 0,
        White = 0,
        Red = 1,
        Greed = 2,
        Blue = 3,
        Tan = 4,
        Gray = 5,
        Black = 6,
        Gold = 7,
        Orange = 8,
        Yellow = 9,
        Gold2 = '=',
        BoldWhite = '-',
        BoldWhite2 = '+',
        DarkGreen = '<',
        DarkGreen2 = ';'
    }
}
