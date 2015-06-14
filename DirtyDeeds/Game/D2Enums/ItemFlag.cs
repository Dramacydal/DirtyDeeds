using System;

namespace DD.Game.Enums
{
    [Flags]
    public enum ItemFlag : uint
    {
        None = 0,
        Equipped = 1,
        // UNKNOWN		= 2,
        // UNKNOWN		= 4,
        InSocket = 8,
        Identified = 0x10,		 // Not undentified, really...
        x20 = 0x20,		 // Has to do with aura / state change !?
        SwitchedIn = 0x40,
        SwitchedOut = 0x80,
        Broken = 0x100,
        // UNKNOWN		= 0x200,
        Potion = 0x400,	 // Set for Mana, Healing and Rejuvenation potions, but not always !?!
        Socketed = 0x800,
        // UNKNOWN		= 0x1000,	 // WasPickedUp ? NOT !
        InStore = 0x2000,	 // Not always set when in store !?
        NotInSocket = 0x4000,	 // Illegal Equip ?
        // UNKNOWN		= 0x8000,
        Ear = 0x10000,	 // Has different packet structure
        StartItem = 0x20000,	 // Item character started with (meaning it's worthless)
        //UNKNOWN		= 0x40000,
        //UNKNOWN		= 0x80000,
        //UNKNOWN		= 0x100000,
        SimpleItem = 0x200000,	 // No ILevel
        Ethereal = 0x400000,
        Any = 0x800000,	 // Which means ??
        Personalized = 0x1000000,
        Gamble = 0x2000000, // Item a town folk is offering for gambling (same purpose as SimpleItem: no ILevel+ info)
        Runeword = 0x4000000,
        x8000000 = 0x8000000, // InducesTempStatusChange ??
        MASK = 0xFE36DF9, // Known / used values
    }
}
