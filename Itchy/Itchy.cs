﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using WhiteMagic;

namespace Itchy
{
    public partial class Itchy : Form
    {
        protected IntPtr keyHookId;
        protected IntPtr mouseHookId;
        protected HookProc hookProc;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public volatile GameSettings Settings = null;
        public volatile OverlaySettings OverlaySettings = null;
        public volatile ItemProcessingSettings ItemProcessingSettings = null;

        private static string ConfigFileName = "Settings.xml";
        private static string OverlayConfigFileName = "OverlaySettings.xml";
        private static string ItemConfigFileName = "Items.ini";

        public Itchy()
        {
            InitializeComponent();

            if (!WinApi.SetDebugPrivileges())
            {
                MessageBox.Show("Failed to set debug privileges. Run as Administrator.");
                return;
            }

            InstallFont();

            //clientsComboBox.DataSource = games;
            UpdateGames();
            UpdateTrayItemList();

            hookProc = new HookProc(HookCallback);
            keyHookId = SetKeyHook(hookProc);
            mouseHookId = SetMouseHook(hookProc);

            ItemProcessingSettings = new ItemProcessingSettings(ItemConfigFileName);
            ItemProcessingSettings.Load();

            LoadSettings();

            clientsToolStripMenuItem.DropDown.Closing += RestrictClosing;

            //var testForm = new TestForm();
            //testForm.Show();
        }

        private void RestrictClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked || !needCloseToolstrip;
            needCloseToolstrip = true;
        }
        protected void LoadSettings()
        {
            StreamReader s = null;
            try
            {
                s = new StreamReader(ConfigFileName);
                var x = new XmlSerializer(typeof(GameSettings));
                Settings = (GameSettings)x.Deserialize(s);
                s.Close();

                s = new StreamReader(OverlayConfigFileName);
                x = new XmlSerializer(typeof(OverlaySettings));
                OverlaySettings = (OverlaySettings)x.Deserialize(s);
                s.Close();
            }
            catch (Exception)
            {
                if (s != null)
                    s.Close();
                Settings = new GameSettings();
                OverlaySettings = new OverlaySettings();
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            if (Settings == null)
                return;

            StreamWriter s = null;
            try
            {
                s = new StreamWriter(ConfigFileName);
                var x = new XmlSerializer(typeof(GameSettings));
                x.Serialize(s, Settings);
                s.Close();

                s = new StreamWriter(OverlayConfigFileName);
                x = new XmlSerializer(typeof(OverlaySettings));
                x.Serialize(s, OverlaySettings);
                s.Close();
            }
            catch (Exception)
            {
                if (s != null)
                    s.Close();
            }
        }

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern IntPtr AddFontMemResourceEx(byte[] pbFont, int cbFont, IntPtr pdv, out uint pcFonts);

        public static Font d2font = null;

        protected void InstallFont()
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
            /*var g = SelectedGame;
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
            }*/
        }

        private void Itchy_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hook.UnhookWindowsHookEx(keyHookId);
            Hook.UnhookWindowsHookEx(mouseHookId);

            foreach (var g in games)
            {
                if (!g.Installed)
                    continue;

                g.Detach();
            }

            SaveSettings();
        }

        private void detachButton_Click(object sender, EventArgs e)
        {
            /*var g = SelectedGame;
            if (g == null)
                return;

            if (g.Installed)
                if (g.Detach())
                    statusLabel.Text = "Detached from " + g.ToString();
                else
                    MessageBox.Show("Failed to detach");

            games.ResetBindings();*/
        }
 
        private void testButton_Click(object sender, EventArgs e)
        {
            /*var g = SelectedGame;
            if (g == null)
                return;

            g.Test();*/
        }

        protected IntPtr SetKeyHook(HookProc proc)
        {
            using (var curProc = Process.GetCurrentProcess())
            using (var curModule = curProc.MainModule)
            {
                return Hook.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, proc, Hook.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        protected IntPtr SetMouseHook(HookProc proc)
        {
            using (var curProc = Process.GetCurrentProcess())
            using (var curModule = curProc.MainModule)
            {
                return Hook.SetWindowsHookEx(HookType.WH_MOUSE_LL, proc, Hook.GetModuleHandle(curModule.ModuleName), 0);
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

                    var foregroundWindow = GetForegroundWindow();
                    if (foregroundWindow != g.Process.MainWindowHandle && foregroundWindow != g.Overlay.Handle)
                        continue;

                    var key = (Keys)Marshal.ReadInt32(lParam);
                    var mEvent = (MessageEvent)wParam.ToInt32();
                    if (!g.HandleMessage(key, mEvent))
                        return 1;   // block
                }
            }
            return Hook.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            games[0].Test();
        }
    }
}
