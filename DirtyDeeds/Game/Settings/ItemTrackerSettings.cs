using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class ItemTrackerSettings : HackSettings
    {
        public ItemTrackerSettings() : base()
        {
            EnablePickit.AddDependency(this);
            UseTelekinesis.AddDependency(EnablePickit);
            EnableTelepick.AddDependency(EnablePickit);
            TeleBack.AddDependency(EnableTelepick);
            TownPick.AddDependency(EnablePickit);

            LogRunes.AddDependency(this);
            LogSets.AddDependency(this);
            LogUniques.AddDependency(this);
            LogItems.AddDependency(this);
        }

        public HackSettings EnablePickit = new HackSettings();
        public HackSettings UseTelekinesis = new HackSettings();
        public HackSettings EnableTelepick = new HackSettings();
        public HackSettings TeleBack = new HackSettings();
        public HackSettings TownPick = new HackSettings();

        public HackSettings LogRunes = new HackSettings();
        public HackSettings LogSets = new HackSettings();
        public HackSettings LogUniques = new HackSettings();
        public HackSettings LogItems = new HackSettings();
        public KeySettings ReactivatePickit = new KeySettings();
    }
}
