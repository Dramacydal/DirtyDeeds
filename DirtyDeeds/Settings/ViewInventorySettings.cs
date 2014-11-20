using System;
using System.Windows.Forms;

namespace DD.Settings
{
    [Serializable]
    public class ViewInventorySettings : HackSettings
    {
        public new static int Cost { get { return 3; } }

        public Keys ViewInventoryKey = Keys.None;
    }
}
