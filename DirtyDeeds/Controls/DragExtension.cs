using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DD
{
    public static class DragExtension
    {
        private static Dictionary<Control, bool> draggables = new Dictionary<Control, bool>();
        private static Point mouseOffs = new Point();

        public static void SetDraggable(this Control control, bool on)
        {
            if (on)
            {
                if (draggables.ContainsKey(control))
                    return;

                control.MouseDown += draggable_MouseDown;
                control.MouseUp += draggable_MouseUp;
                control.MouseMove += draggable_MouseMove;

                draggables.Add(control, false);
            }
            else
            {
                if (!draggables.ContainsKey(control))
                    return;

                control.MouseDown -= draggable_MouseDown;
                control.MouseUp -= draggable_MouseUp;
                control.MouseMove -= draggable_MouseMove;

                draggables.Remove(control);
            }
        }

        static void draggable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mouseOffs = e.Location;
                draggables[(Control)sender] = true;
            }
        }

        static void draggable_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                draggables[(Control)sender] = false;
        }

        static void draggable_MouseMove(object sender, MouseEventArgs e)
        {
            var control = (Control)sender;

            if (draggables[control])
            {
                var newLocOffs = new Point
                {
                    X = e.Location.X - mouseOffs.X,
                    Y = e.Location.Y - mouseOffs.Y
                };

                control.RelocateByDrag(newLocOffs);
            }
        }

        static void RelocateByDrag(this Control control, Point offset)
        {
            if (control.Parent != null && !(control.Parent is Form))
            {
                control.Parent.RelocateByDrag(offset);
                return;
            }

            control.Left += offset.X;
            control.Top += offset.Y;

            if (control is DirtyDeedsControls.ExpandButton)
            {
                var butt = control as DirtyDeedsControls.ExpandButton;
                if (butt.ChildPanel != null)
                    butt.ChildPanel.RelocateByDrag(offset);
            }
        }
    }
}
