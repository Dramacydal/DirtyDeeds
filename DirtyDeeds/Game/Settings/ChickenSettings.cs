using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class ChickenHackSettings : HackSettings
    {
        public HackSettings ChickenToTown = new HackSettings();
        public HackSettings ChickenOnHostility = new HackSettings();
        public double ChickenLifePercent = 0.0f;
        public double ChickenManaPercent = 0.0f;
    }
}
