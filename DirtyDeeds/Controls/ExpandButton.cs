using System;
using System.Windows.Forms;

namespace DirtyDeedsControls
{
    public partial class ExpandButton : Button
    {
        public bool Expanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                if (childPanel != null)
                {
                    if (expanded)
                        childPanel.Show();
                    else
                        childPanel.Hide();
                }
                UpdateText();
            }
        }

        public TranslucentPanel ChildPanel
        {
            get { return childPanel; }
            set
            {
                childPanel = value;
                if (Expanded)
                    childPanel.Show();
                else
                    childPanel.Hide();
            }
        }

        protected TranslucentPanel childPanel;
        protected bool expanded;

        public string ExpandedString = "\\/";
        public string HiddenString = "/\\";

        public ExpandButton() : base()
        {
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (base.Text.Contains(HiddenString))
                {
                    if (Expanded)
                        base.Text = base.Text.Replace(HiddenString, ExpandedString);
                }
                else if (Text.Contains(ExpandedString))
                {
                    if (!Expanded)
                        base.Text = base.Text.Replace(ExpandedString, HiddenString);
                }
                else if (Expanded)
                    base.Text = base.Text + " " + ExpandedString;
                else
                    base.Text = base.Text + " " + HiddenString;
            }
        }

        protected void UpdateText()
        {
            Text = Text;
        }

        protected override void OnClick(EventArgs e)
        {
            if (childPanel != null)
                Expanded = !childPanel.Visible;

            base.OnClick(e);
        }
    }
}
