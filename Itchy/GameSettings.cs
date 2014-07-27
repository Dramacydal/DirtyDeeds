using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhiteMagic;

namespace Itchy
{
    [Serializable]
    public class HackSettings
    {
        public bool Enabled = false;

        public HackSettings() { }
    }

    [Serializable]
    public class KeySettings
    {
        public Keys Key = Keys.None;

        public KeySettings() { }
    }

    [Serializable]
    public class PacketReceivedHackSettings : HackSettings
    {
        public bool BlockFlash = false;
        public bool FastTele = false;
        public bool FastPortal = false;
    }

    [Serializable]
    public class FastPortalSettings : KeySettings
    {
        public bool GoToTown = false;
    }

    [Serializable]
    public class ChickenSettings : HackSettings
    {
        public bool ChickenToTown = false;
        public bool ChickenOnHostile = false;
        public double ChickenLifePercent = 0.0f;
        public double ChickenManaPercent = 0.0f;
    }

    [Serializable]
    public class GameSettings
    {
        public GameSettings() { }

        public HackSettings LightHack = new HackSettings();
        public HackSettings WeatherHack = new HackSettings();
        public PacketReceivedHackSettings ReceivePacketHack = new PacketReceivedHackSettings();

        public KeySettings RevealAct = new KeySettings();
        public KeySettings OpenStash = new KeySettings();
        public KeySettings OpenCube = new KeySettings();
        public KeySettings FastExit = new KeySettings();
        public FastPortalSettings FastPortal = new FastPortalSettings();
        public ChickenSettings Chicken = new ChickenSettings();
    }
}
