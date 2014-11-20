using System;

namespace DD.Settings
{
    [Serializable]
    public class InfravisionSettings : HackSettings
    {
        public bool HideCorpses = false;
        public bool HideDying = false;
        public bool HideItems = false;
    }
}
