using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Win32HWBP;

namespace Itchy
{
    public partial class Itchy : Form
    {
        public Itchy()
        {
            InitializeComponent();

            UpdateGames();

            if (!WinApi.SetDebugPrivileges())
            {
                MessageBox.Show("Failed to set debug privileges. Run as Administrator.");
                return;
            }

            clientsComboBox.DataSource = games;
        }

        private void attachButton_Click(object sender, EventArgs e)
        {
            var g = SelectedGame;
            if (g == null)
                return;

            if (g.Installed)
                return;

            if (!g.Install())
                MessageBox.Show("Failed to install hack");
            else
            {
                statusLabel.Text = "Attached to " + g.ToString();
                games.ResetBindings();
            }
        }

        private void Itchy_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var g in Games)
            {
                if (!g.Installed)
                    continue;

                g.Detach();
            }
        }

        private void detachButton_Click(object sender, EventArgs e)
        {
            var g = SelectedGame;
            if (g == null)
                return;

            if (g.Installed)
                if (g.Detach())
                    statusLabel.Text = "Detached from " + g.ToString();
                else
                    MessageBox.Show("Failed to detach");

            games.ResetBindings();
        }

        private void clientsComboBox_DropDown(object sender, EventArgs e)
        {
            UpdateGames();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var g = SelectedGame;
            if (g == null)
                return;

            g.Test();
        }
    }
}
