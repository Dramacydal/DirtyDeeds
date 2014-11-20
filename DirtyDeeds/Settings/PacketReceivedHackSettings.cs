using System;

namespace DD.Settings
{
    [Serializable]
    public class PacketReceivedHackSettings : HackSettings
    {
        public bool BlockFlash = false;
        public bool FastTele = false;
        public bool FastPortal = false;
        public ItemTrackerSettings ItemTracker = new ItemTrackerSettings();
    }
}
