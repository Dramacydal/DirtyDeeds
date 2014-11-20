using System;

namespace DD.Settings
{
    [Serializable]
    public class GameSettings
    {
        public GameSettings() { }

        public HackSettings LightHack = new HackSettings();
        public HackSettings WeatherHack = new HackSettings();
        public PacketReceivedHackSettings ReceivePacketHack = new PacketReceivedHackSettings();
        public ItemNameHackSettings ItemNameHack = new ItemNameHackSettings();
        public ViewInventorySettings ViewInventory = new ViewInventorySettings();
        public InfravisionSettings Infravision = new InfravisionSettings();

        public KeySettings RevealAct = new KeySettings();
        public KeySettings OpenStash = new KeySettings();
        public KeySettings OpenCube = new KeySettings();
        public KeySettings FastExit = new KeySettings();
        public FastPortalSettings FastPortal = new FastPortalSettings();
        public ChickenSettings Chicken = new ChickenSettings();

        public KeySettings AutoteleNext = new KeySettings();
        public KeySettings AutoteleMisc = new KeySettings();
        public KeySettings AutoteleWP = new KeySettings();
        public KeySettings AutotelePrev = new KeySettings();
    }
}
