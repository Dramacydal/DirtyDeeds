namespace DD.D2Enums
{
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
}
