using System;

namespace DD.Settings
{
    [Serializable]
    public class ItemTrackerSettings : HackSettings
    {
        public bool EnablePickit = false;
        public bool UseTelekinesis = false;
        public bool EnableTelepick = false;
        public bool TeleBack = false;
        public bool TownPick = false;

        public bool LogRunes = false;
        public bool LogSets = false;
        public bool LogUniques = false;
        public bool LogItems = false;
        public KeySettings ReactivatePickit = new KeySettings();
    }
}
