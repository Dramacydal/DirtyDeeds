using System;
using System.Windows.Forms;
using DD;
using DD.Game;

namespace DirtyDeedsControls
{
    public enum D2ToolstripType
    {
        AttachToAll = 0,
        DetachFromAll = 1,
        Game = 2
    }

    public partial class DDToolstripMenuItem : ToolStripMenuItem
    {
        public D2Game Game { get; protected set; }
        public D2ToolstripType Type { get; protected set; }

        public DDToolstripMenuItem(D2Game game)
            : base()
        {
            this.CheckOnClick = true;
            this.Game = game;
            this.Type = D2ToolstripType.Game;
            this.Text = game.ToString();
            this.Checked = game.Installed;
        }

        public DDToolstripMenuItem(D2ToolstripType type)
            : base()
        {
            this.CheckOnClick = false;
            this.Game = null;
            this.Type = type;

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
            if (Game == null || Game.Installed)
                return;

            if (!Game.Install())
                MessageBox.Show("Failed to install hack");
            else
                this.Text = Game.ToString();

            this.Checked = Game.Installed;
        }

        public void Detach()
        {
            if (Game == null || !Game.Installed)
                return;

            if (Game.Detach())
                this.Text = Game.ToString();
            else
                MessageBox.Show("Failed to detach");

            this.Checked = Game.Installed;
        }

        protected override void OnClick(EventArgs e)
        {
            if (Game == null)
            {
                base.OnClick(e);
                return;
            }

            if (Game.Installed)
                Detach();
            else
                Attach();
        }
    }
}
