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
    public class PacketReceivedHackSettings : HackSettings
    {
        public bool BlockFlash = false;
        public bool FastTele = false;
        public bool FastPortal = false;
    }

    [Serializable]
    public class GameSettings
    {
        public GameSettings() { }

        public HackSettings LightHack = new HackSettings();
        public HackSettings WeatherHack = new HackSettings();
        public PacketReceivedHackSettings ReceivePacketHack = new PacketReceivedHackSettings();

        public Keys RevealActKey = Keys.None;
        public Keys OpenStashKey = Keys.None;
        public Keys OpenCubeKey = Keys.None;
        public Keys FastExitKey = Keys.None;
    }
}
