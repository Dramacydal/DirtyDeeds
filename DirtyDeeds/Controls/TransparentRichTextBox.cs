using System.Windows.Forms;

namespace DirtyDeedsControls
{
    public class TransparentRichTextBox : RichTextBox
    {
        public TransparentRichTextBox() : base()
        {
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
        }
    }
}
