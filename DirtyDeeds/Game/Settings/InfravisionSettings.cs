using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class InfravisionHackSettings : HackSettings
    {
        public InfravisionHackSettings() : base()
        {
            HideCorpses.AddDependency(this);
            HideDying.AddDependency(this);
            HideItems.AddDependency(this);
        }

        public HackSettings HideCorpses = new HackSettings();
        public HackSettings HideDying = new HackSettings();
        public HackSettings HideItems = new HackSettings();
    }
}
