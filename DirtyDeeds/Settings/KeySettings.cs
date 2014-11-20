using System;
using System.Windows.Forms;

namespace DD.Settings
{
    [Serializable]
    public class KeySettings
    {
        public KeySettings() { }

        public Keys Key = Keys.None;
    }
}
