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
        Purple = ';',

        Default = White,
    }

    public enum UIVars : uint
    {
        Unk0 = 0x00, // always 1
        Inventory = 0x01, // hotkey 'B'
        Stats = 0x02, // hotkey 'C'
        CurrSkill = 0x03, // left or right hand skill selection
        Skills = 0x04, // hotkey 'T'
        ChatInput = 0x05, // chat with other players, hotkey ENTER
        NewStats = 0x06, // new stats button
        NewSkill = 0x07, // new skills button
        Interact = 0x08, // interact with NPC
        GameMenu = 0x09, // hotkey ESC
        AutoMap = 0x0A, // hotkey TAB
        CfgCtrls = 0x0B, // config control key combination
        NpcTrade = 0x0C, // trade, game, repair with NPC
        ShowItems = 0x0D, // hotkey ALT
        ModItem = 0x0E, // add socket, imbue item
        Quest = 0x0F, // hotkey 'Q'
        Unk16 = 0x10,
        QuestLog = 0x11, // quest log button on the bottom left screen
        StatusArea = 0x12, // lower panel can not redraw when set
        Unk19 = 0x13, // init 1
        Waypoint = 0x14,
        Minipanel = 0x15,
        Party = 0x16, // hotkey 'P'
        PplTrade = 0x17, // trade, exchange items with other player
        MsgLog = 0x18,
        Stash = 0x19,
        Cube = 0x1A,
        Unk27 = 0x1B,
        Inventory2 = 0x1C,
        Inventory3 = 0x1D,
        Inventory4 = 0x1E,
        Belt = 0x1F,
        Unk32 = 0x20,
        Help = 0x21, // help screen, hotkey 'H'
        Unk34 = 0x22,
        Unk35 = 0x23, // init 1
        Pet = 0x24, // hotkey 'O'
        QuestScroll = 0x25, // for showing quest information when click quest item.

        Max = QuestScroll,
    }

    public enum StorageType : uint
    {
        Inventory = 0,
        Equip = 1,
        Trade = 2,
        Cube = 3,
        Stash = 4,
        Belt = 5,
        Null = 255,
    }

    public enum Nodepage : uint
    {
        Storage = 1,
        BeltSlots = 2,
        Equip = 3
    }

    public enum Stat : uint
    {
        Strength = 0, // = str
        Energy = 1, // = energy
        Dexterity = 2, // = dexterity
        Vitality = 3, // = vitality
        Health = 6,
        MaxHealth = 7,
        Mana = 8, // = mana
        MaxMana = 9, // = max = mana
        Stamina = 10, // = stamina
        MaxStamina = 11, // = max = stamina
        Exp = 13, // = experience
        GgoldBank= 15, // = stash = gold
        ToBlock = 20, // = to = block
        MaxMagicResist = 38, // = max = magic = resist
        MaxFireResist = 40, // = max = fire = resist
        MaxLightningResist = 42, // = max = lightning = resist
        MaxColdResist = 44, // = max = cold = resist
        MaxPoisonResist = 46, // = max = poison = resist
        VelocityPercent = 67, // = effective = run/walk
        AmmoQuantity = 70, // = ammo = quantity(arrow/bolt/throwing)
        Durability = 72, // = item = durability
        MaxDurability = 73, // = max = item = durability
        PoisonLengthReduction = 110, // = Poison = length = reduction
        OpenWounds = 135, // = Open = Wounds
        DeadlyStrike = 141, // = deadly = strike
        FireAbsorbPercent = 142, // = fire = absorb = %
        LightningAbsorbPercent = 144, // = lightning = absorb = %
        ColdAbsorbPercent = 148, // = cold = absorb = %
        ColdAbsorb = 149, // = cold = absorb
        Slow = 150, // = slow = %
        Sockets = 194,
    }

    public enum ItemQuality : uint
    {
        Any = 0,    // custom value
        Inferior = 1,
        Normal = 2,
        Superior = 3,
        Magic = 4,
        Set = 5,
        Rare = 6,
        Unique = 7,
        Craft = 8,
    }

    //public enum Item
}