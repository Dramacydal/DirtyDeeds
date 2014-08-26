using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItchyControls
{
    public partial class DraggablePanel : Panel
    {
        public bool BeingDragged { get { return beingDragged; } }

        protected bool beingDragged = false;
        protected Point mouseOffs = new Point();

        public DraggablePanel()
            : base()
        {
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                beingDragged = true;
                mouseOffs = e.Location;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                beingDragged = false;

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (beingDragged)
            {
                var newLocOffs = new Point
                {
                    X = e.Location.X - mouseOffs.X,
                    Y = e.Location.Y - mouseOffs.Y
                };

                this.Left += newLocOffs.X;
                this.Top += newLocOffs.Y;
            }

            base.OnMouseMove(e);
        }
    }
}
