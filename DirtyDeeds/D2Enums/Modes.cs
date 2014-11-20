namespace DD.D2Enums
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

    public enum NpcMode : uint
    {
        Death = 0,
        Stand = 1,
        Walk = 2,
        BeingHit = 3,
        Attack1 = 4,
        Attack2 = 5,
        Block = 6,
        Cast = 7,
        UseSkill1 = 8,
        UseSkill2 = 9,
        UseSkill3 = 10,
        UseSkill4 = 11,
        Dead = 12,
        BeingKnockBack = 13,
        Sequence = 14,
        Run = 15,
    }

}
