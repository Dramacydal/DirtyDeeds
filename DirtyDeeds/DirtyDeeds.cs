using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using DirtyDeedsControls;
using WhiteMagic;
using DD.Game.Settings;
using DD.Tools;
using DD.Game;

namespace DD
{
    public partial class DirtyDeeds : Form
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

        public DirtyDeeds()
        {
            InitializeComponent();

            if (!MagicHelpers.SetDebugPrivileges())
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

        public List<D2Game> games = new List<D2Game>();

        protected void UpdateGames()
        {
            games.ForEach(g =>
            {
                if (!g.Exists())
                    g.Dispose();
            });
            games.RemoveAll(g => !g.Exists());

            var processes = MagicHelpers.FindProcessesByInternalName("diablo ii");
            foreach (var process in processes)
            {
                if (games.Any(g => g.Process.Id == process.Id))
                    continue;

                games.Add(new D2Game(process, this));
            }
        }

        private void clientsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            UpdateGames();
            UpdateTrayItemList();
        }

        bool needCloseToolstrip = true;
        protected void UpdateTrayItemList()
        {
            clientsToolStripMenuItem.DropDownItems.Clear();

            var attachItem = new DDToolstripMenuItem(D2ToolstripType.AttachToAll);
            attachItem.Click += (sender, e) =>
            {
                foreach (var obj in clientsToolStripMenuItem.DropDownItems)
                {
                    if (obj.GetType() != typeof(DDToolstripMenuItem))
                        continue;

                    var item = (DDToolstripMenuItem)obj;
                    if (item.Type != D2ToolstripType.Game)
                        continue;

                    item.Attach();
                }
            };

            var detachItem = new DDToolstripMenuItem(D2ToolstripType.DetachFromAll);
            detachItem.Click += (sender, e) =>
            {
                foreach (var obj in clientsToolStripMenuItem.DropDownItems)
                {
                    if (obj.GetType() != typeof(DDToolstripMenuItem))
                        continue;

                    var item = (DDToolstripMenuItem)obj;
                    if (item.Type != D2ToolstripType.Game)
                        continue;

                    item.Detach();
                }
            };

            if (games.Count == 0)
            {
                attachItem.Enabled = false;
                detachItem.Enabled = false;
            }

            clientsToolStripMenuItem.DropDownItems.Add(attachItem);
            clientsToolStripMenuItem.DropDownItems.Add(detachItem);
            clientsToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

            foreach (var g in games)
            {
                var t = new DDToolstripMenuItem(g);
                t.MouseDown += (sender, e) =>
                {
                    needCloseToolstrip = false;
                };

                clientsToolStripMenuItem.DropDownItems.Add(t);
            }
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
            }
            catch
            {
                Settings = new GameSettings();
                OverlaySettings = new OverlaySettings();
                SaveSettings();
            }
            finally
            {
                if (s != null)
                    s.Close();
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
            }
            catch
            {
            }
            finally
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

        private void DirtyDeeds_FormClosing(object sender, FormClosingEventArgs e)
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
                var key = (Keys)Marshal.ReadInt32(lParam);
                var mEvent = (MessageEvent)wParam.ToInt32();

                foreach (D2Game g in games)
                {
                    if (g.Overlay == null)
                        continue;

                    var foregroundWindow = GetForegroundWindow();
                    if (foregroundWindow != g.Process.MainWindowHandle && foregroundWindow != g.Overlay.Handle)
                        continue;

                    if (!g.HandleMessage(key, mEvent))
                        return 1;   // block
                }

                if (settingsForm != null && mEvent == MessageEvent.WM_KEYUP)
                    if (!settingsForm.HandleMessage(key, mEvent))
                        return 1;
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


        SettingsForm settingsForm = null;

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(this))
            {
                this.settingsForm = settingsForm;

                var result = settingsForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var settings = settingsForm.ProduceSettings();
                    this.Settings = settings;

                    ApplySettings();
                }

                this.settingsForm = null;
            }
        }

        public void ApplySettings()
        {
            foreach (var game in games)
                if (game.Installed)
                    game.ApplySettings();
        }
    }
}
