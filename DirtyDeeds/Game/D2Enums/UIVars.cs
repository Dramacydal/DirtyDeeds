namespace DD.Game.Enums
{
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
}
