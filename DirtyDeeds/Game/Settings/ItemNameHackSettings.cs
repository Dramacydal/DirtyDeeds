using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class ItemNameHackSettings : HackSettings
    {
        public ItemNameHackSettings() : base()
        {
            ShowEth.AddDependency(this);
            ShowItemLevel.AddDependency(this);
            ShowItemPrice.AddDependency(this);
            ShowRuneNumber.AddDependency(this);
            ShowSockets.AddDependency(this);
            ShowItemCode.AddDependency(this);
            ChangeItemColor.AddDependency(this);
        }

        public HackSettings ShowEth = new HackSettings();
        public HackSettings ShowItemLevel = new HackSettings();
        public HackSettings ShowItemPrice = new HackSettings();
        public HackSettings ShowRuneNumber = new HackSettings();
        public HackSettings ShowSockets = new HackSettings();
        public HackSettings ShowItemCode = new HackSettings();
        public HackSettings ChangeItemColor = new HackSettings();
    }
}
