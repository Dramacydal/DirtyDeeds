using System;
using System.Windows.Forms;
using DD;

namespace DirtyDeedsControls
{
    public enum D2ToolstripType
    {
        AttachToAll = 0,
        DetachFromAll = 1,
        Game = 2
    }

    public partial class D2ToolstripMenuItem : ToolStripMenuItem
    {
        public D2Game Game { get { return game; } }
        public D2ToolstripType Type { get { return type; } }

        protected D2Game game;
        protected D2ToolstripType type;

        public D2ToolstripMenuItem(D2Game game)
            : base()
        {
            this.CheckOnClick = true;
            this.game = game;
            this.type = D2ToolstripType.Game;
            this.Text = game.ToString();
            this.Checked = game.Installed;
        }

        public D2ToolstripMenuItem(D2ToolstripType type)
            : base()
        {
            this.CheckOnClick = false;
            this.game = null;
            this.type = type;

            switch (type)
            {
                case D2ToolstripType.AttachToAll:
                    this.Text = "Attach to all";
                    break;
                case D2ToolstripType.DetachFromAll:
                    this.Text = "Detach from all";
                    break;
                default:
                    this.Text = "-";
                    break;
            }
        }

        public void Attach()
        {
            if (game == null || game.Installed)
                return;

            if (!game.Install())
                MessageBox.Show("Failed to install hack");
            else
                this.Text = game.ToString();

            this.Checked = game.Installed;
        }

        public void Detach()
        {
            if (game == null || !game.Installed)
                return;

            if (game.Detach())
                this.Text = game.ToString();
            else
                MessageBox.Show("Failed to detach");

            this.Checked = game.Installed;
        }

        protected override void OnClick(EventArgs e)
        {
            if (game == null)
            {
                base.OnClick(e);
                return;
            }

            if (game.Installed)
                Detach();
            else
                Attach();
        }
    }
}
