using System.Drawing;
using System.Windows.Forms;

namespace DirtyDeedsControls
{
    public class TranslucentPanel : Panel
    {
        public TranslucentPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }
    }
}
