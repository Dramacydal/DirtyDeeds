﻿using System;
using System.Drawing;

namespace DD.Game.Settings
{
    [Serializable]
    public class OverlaySettings
    {
        public OverlaySettings() { }

        public float LogFontSize = 0.0f;

        public Point LogPosition = new Point();
        public Point StatsPosition = new Point();
    }
}
