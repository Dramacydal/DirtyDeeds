using System;
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
                this.Text = key.ToString();
                WaitingKeyPress = false;
            }
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
