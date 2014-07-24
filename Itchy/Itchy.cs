using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WhiteMagic;

namespace Itchy
{
    public partial class Itchy : Form
    {
        protected IntPtr hookId;
        protected HookProc _proc;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public Itchy()
        {
            InitializeComponent();

            if (!WinApi.SetDebugPrivileges())
            {
                MessageBox.Show("Failed to set debug privileges. Run as Administrator.");
                return;
            }

            InstallFont();

            clientsComboBox.DataSource = games;
            UpdateGames();

            _proc = new HookProc(HookCallback);
            hookId = SetHook(_proc);

            var panel1 = new Form1();
            panel1.Show();
        }

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern IntPtr AddFontMemResourceEx(byte[] pbFont, int cbFont, IntPtr pdv, out uint pcFonts);

        public static Font d2font = null;

        private void InstallFont()
        {
            var data = Properties.Resources.diablo_h;

            uint cFonts;
            AddFontMemResourceEx(data, data.Length, IntPtr.Zero, out cFonts);

            var ptr = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            var pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(ptr, data.Length);
            var ff = pfc.Families[0];
            d2font = new Font(ff, 15f, FontStyle.Bold);
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
            Hook.UnhookWindowsHookEx(hookId);

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

        protected IntPtr SetHook(HookProc proc)
        {
            using (var curProc = Process.GetCurrentProcess())
            using (var curModule = curProc.MainModule)
            {
                return Hook.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, proc, Hook.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                foreach (D2Game g in games)
                {
                    if (g.Overlay == null)
                        continue;

                    if (!g.Overlay.TopMost && GetForegroundWindow() != g.Process.MainWindowHandle)
                        continue;

                    if (!g.Overlay.HandleMessage(code, wParam, lParam))
                        return 1;   // block
                }
            }
            return Hook.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
    }
}
