namespace Itchy.D2Api
{
    //TODO: 2 unknowns left... are there imbue and craft / transmute action types ?
    public enum ItemActionType
    {
        AddToGround = 0,
        GroundToCursor = 1,			// only sent if item goes to cursor (GS packet 0x0A removes item from ground)
        DropToGround = 2,
        OnGround = 3,
        PutInContainer = 4,
        RemoveFromContainer = 5,
        Equip = 6,
        /// <summary>
        /// Sent for the equipped item when changing from a two handed weapon to a single handed weapon or vice versa.
        /// The item must be equiped on the "empty" hand or a regular SwapBodyItem will be sent instead.
        /// Empty hand meaning left hand if currently wearing a two handed weapon or the empty hand if wearing a single hand item.
        /// The result will be the new item being equipped and the old going to cursor.
        /// </summary>
        IndirectlySwapBodyItem = 7,
        Unequip = 8,
        SwapBodyItem = 9,
        AddQuantity = 0x0A,
        AddToShop = 0x0B,
        RemoveFromShop = 0x0C,
        SwapInContainer = 0x0D,
        PutInBelt = 0x0E,
        RemoveFromBelt = 0x0F,
        SwapInBelt = 0x10,
        /// <summary>
        /// Sent for the secondary hand's item going to inventory when changing from a dual item setup to a two handed weapon.
        /// </summary>
        AutoUnequip = 0x11,
        RemoveFromHireling = 0x12,	// sent along with a 9d 08 packet... Also Item on cursor when entering game ?? MiscToCursor??
        ItemInSocket = 0x13,
        UNKNOWN1 = 0x14,
        UpdateStats = 0x15,			// put item in socket; for each potion that drops in belt when lower one is removed...
        UNKNOWN2 = 0x16,
        WeaponSwitch = 0x17,
    }
}
