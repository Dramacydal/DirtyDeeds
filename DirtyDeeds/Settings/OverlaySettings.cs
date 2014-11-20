using System;
using System.Drawing;

namespace DD.Settings
{
    [Serializable]
    public class OverlaySettings
    {
        public OverlaySettings() { }

        public float LogFontSize = 0.0f;

        public Point SettingsPosition = new Point();
        public Point LogPosition = new Point();
        public Point StatsPosition = new Point();
    }
}
