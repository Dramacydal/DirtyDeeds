namespace Itchy.D2Api
{
    /// <summary>
    /// This is used by GS Packet 0x9C - 0x9D, and is pretty weird...
    /// </summary>
    public enum ItemCategory
    {
        Helm = 0,
        Armor = 1,
        /// <summary>
        /// Most weapons, including Crossbows
        /// </summary>
        Weapon = 5,
        /// <summary>
        /// Bows (not crossbows), sometimes shield (if equiped in LeftHand?)
        /// </summary>
        Weapon2 = 6,
        /// <summary>
        /// Shields can some somtimes be Weapon2...
        /// </summary>
        Shield = 7,
        /// <summary>
        /// Class specific items !?
        /// </summary>
        Special = 10,
        /// <summary>
        /// BaseMiscItems and gloves, boots...
        /// </summary>
        Misc = 16,
    }
}
