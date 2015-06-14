using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class PacketReceivedHackSettings : HackSettings
    {
        public PacketReceivedHackSettings() : base()
        {
            BlockFlash.AddDependency(this);
            FastTele.AddDependency(this);
            NoTownPortalAnim.AddDependency(this);
            ItemTracker.AddDependency(this);
        }

        public HackSettings BlockFlash = new HackSettings();
        public HackSettings FastTele = new HackSettings();
        public HackSettings NoTownPortalAnim = new HackSettings();
        public ItemTrackerSettings ItemTracker = new ItemTrackerSettings();
    }
}
