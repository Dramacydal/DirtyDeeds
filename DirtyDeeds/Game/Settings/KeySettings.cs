using System;
using System.Windows.Forms;

namespace DD.Game.Settings
{
    [Serializable]
    public class KeySettings
    {
        public KeySettings() { }

        public Keys Key = Keys.None;
    }
}
