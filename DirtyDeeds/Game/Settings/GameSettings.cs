using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class GameSettings
    {
        public GameSettings()
        {
            GoToTownAfterPortal.AddDependency(ReceivePacketHack);
            Chicken.AddDependency(ReceivePacketHack);
        }

        public HackSettings LightHack = new HackSettings();
        public HackSettings WeatherHack = new HackSettings();
        public PacketReceivedHackSettings ReceivePacketHack = new PacketReceivedHackSettings();
        public ItemNameHackSettings ItemNameHack = new ItemNameHackSettings();
        public ViewInventoryHackSettings ViewInventory = new ViewInventoryHackSettings();
        public InfravisionHackSettings Infravision = new InfravisionHackSettings();
        public ChickenHackSettings Chicken = new ChickenHackSettings();

        public KeySettings RevealAct = new KeySettings();
        public KeySettings OpenStash = new KeySettings();
        public KeySettings OpenCube = new KeySettings();
        public KeySettings FastExit = new KeySettings();
        public KeySettings FastPortal = new KeySettings();
        public HackSettings GoToTownAfterPortal = new HackSettings();

        public KeySettings AutoteleNext = new KeySettings();
        public KeySettings AutoteleMisc = new KeySettings();
        public KeySettings AutoteleWP = new KeySettings();
        public KeySettings AutotelePrev = new KeySettings();
    }
}
