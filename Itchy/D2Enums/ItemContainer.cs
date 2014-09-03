namespace Itchy.D2Enums
{
    public enum ItemContainer
    {
        Equipment = 0x00,	// Player or Merc Equipment
        Ground = 0x01,
        Inventory = 0x02,
        TraderOffer = 0x04,
        ForTrade = 0x06,
        Cube = 0x08,
        Stash = 0x0A,
        // Not a buffer... if (buffer == Equipement && Location == EquipmentLocation.NotApplicable)
        Belt = 0x0C,
        // Not a buffer... if (buffer == Equipement && destination == ItemDestination.Item)
        Item = 0x0E,
        //NPC buffers are flagged with 0x80 so they are different
        ArmorTab = 0x82,
        WeaponTab1 = 0x84,
        WeaponTab2 = 0x86,
        MiscTab = 0x88,
        //ArmorTabBottom	= 0x83, // Buffer merged with ArmorTab
        //WeaponTab1Bottom	= 0x85, // Buffer merged with WeaponTab1
        //MiscTabBottom		= 0x89, // Buffer merged with WeaponTab2
    }
}
