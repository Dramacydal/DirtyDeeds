using System;

namespace DD.Settings
{
    [Serializable]
    public class ItemNameHackSettings : HackSettings
    {
        public bool ShowEth = false;
        public bool ShowItemLevel = false;
        public bool ShowItemPrice = false;
        public bool ShowRuneNumber = false;
        public bool ShowSockets = false;
        public bool ShowItemCode = false;
        public bool ChangeItemColor = false;
    }
}
