using System;

namespace DD.Game.Settings
{
    [Serializable]
    public class HackSettings
    {
        protected HackSettings Parent = null;

        public HackSettings() { }

        public bool Enabled = false;
        public static int Cost { get { return 1; } }

        public void AddDependency(HackSettings parent)
        {
            this.Parent = parent;
        }

        public bool IsEnabled()
        {
            return Enabled && (Parent == null || Parent.IsEnabled());
        }
    }
}
