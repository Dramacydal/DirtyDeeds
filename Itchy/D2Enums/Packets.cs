namespace Itchy.D2Enums
{
    public enum GameClientPacket : byte
    {
        CastRightSkill = 0x0C,
        CastRightSkillOnTarget = 0x0D,
        PickItem = 0x16,
        SelectSkill = 0x3C,
    }

    public enum GameServerPacket : byte
    {
        RemoveGroundUnit = 0x0A,
        GameObjectModeChange = 0x0E,
        PlayerReassign = 0x15,
        AttributeByte = 0x1D,
        AttributeWord = 0x1E,
        AttributeDWord = 0x1F,
        StateNotification = 0x20,
        TriggerSound = 0x2C,
        Unknown18 = 0x18,
        GameObjectAssignment = 0x51,
        PlayerInfomation = 0x5A,
        PlayerLifeManaChange = 0x95,
        WorldItemAction = 0x9C,
        OwnedItemAction = 0x9D,
        DelayedState = 0xA7,
    }
}
