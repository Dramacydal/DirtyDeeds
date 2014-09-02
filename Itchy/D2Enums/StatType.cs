namespace Itchy.D2Enums
{
    public enum StatType : uint
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
        Level = 12,
        Exp = 13, // = experience
        GoldBank = 15, // = stash = gold
        ToHit = 19,
        ToBlock = 20, // = to = block
        DamageReduction = 34,
        MagicDamageReduction = 35,
        DamageResist = 36,
        MagicResist = 37,
        MaxMagicResist = 38, // = max = magic = resist
        FireResist = 39,
        MaxFireResist = 40, // = max = fire = resist
        LightResist = 41,
        MaxLightningResist = 42, // = max = lightning = resist
        ColdResist = 43,
        MaxColdResist = 44, // = max = cold = resist
        PoisonResist = 45,
        MaxPoisonResist = 46, // = max = poison = resist
        VelocityPercent = 67, // = effective = run/walk
        AmmoQuantity = 70, // = ammo = quantity(arrow/bolt/throwing)
        Durability = 72, // = item = durability
        MaxDurability = 73, // = max = item = durability
        GoldFind = 79,
        MagicFind = 80,
        FasterAttackRate = 93,
        FasterMoveVelocity = 96,
        FasterHitRecovery = 99,
        FasterBlockRate = 102,
        FasterCastRate = 105,
        PoisonLengthReduction = 110, // = Poison = length = reduction
        OpenWounds = 135, // = Open = Wounds
        CrushingBlow = 136,
        DeadlyStrike = 141, // = deadly = strike
        AbsorbFirePercent = 142,
        AbsorbFire = 143,
        AbsorbLightingPercent = 144, // = lightning = absorb = %
        AbsorbLight = 145,
        AbsorbMagicPercent = 146,
        AbsorbMagic = 147,
        AbsorbColdPercent = 148, // = cold = absorb = %
        AbsorbCold = 149, // = cold = absorb
        Slow = 150, // = slow = %
        Sockets = 194,
    }
}
