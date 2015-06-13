using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DirtyDeedsControls
{
    public partial class KeybindButton : Button
    {
        public Keys Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                this.Text = ProduceName(key);
                WaitingKeyPress = false;
            }
        }

        protected static string ProduceName(Keys key)
        {
            var value = key.ToString();
            value = value.Replace("NumPad", "Num ");
            value = Regex.Replace(value, "^D([0-9])$", "$1");

            return value;
        }

        public bool WaitingKeyPress { get; set; }

        protected Keys key = Keys.None;

        public KeybindButton()
            : base()
        {
            Key = Keys.None;
            WaitingKeyPress = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            Reset();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (WaitingKeyPress)
                return;

            this.Text = "...";
            WaitingKeyPress = true;
        }

        public void Reset()
        {
            WaitingKeyPress = false;
            Key = Key;
        }
    }
}
