using System;

namespace DD.Settings
{
    [Serializable]
    public class ChickenSettings : HackSettings
    {
        public bool ChickenToTown = false;
        public bool ChickenOnHostile = false;
        public double ChickenLifePercent = 0.0f;
        public double ChickenManaPercent = 0.0f;
    }
}
