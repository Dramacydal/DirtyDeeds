using System;

namespace DD.Settings
{
    [Serializable]
    public class HackSettings
    {
        public HackSettings() { }

        public bool Enabled = false;
        public static int Cost { get { return 1; } }
    }
}
