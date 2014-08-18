using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using ItchyControls;

namespace Itchy
{
    public partial class OverlayWindow : Form
    {
        public D2Game game { get; set; }

        public bool ClickThrough { get; set; }

        public OverlayWindow()
        {
            InitializeComponent();
        }

        //
        public void PostCreate()
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            MakeTransparent(true);
            MakeNonInteractive(true);

            SetupSettings(game.Settings);

            SetupControlPositions();

            //translucentPanel1.BackColor = Color.FromArgb(127, Color.White);

            //this.BackColor = Color.FromArgb(127, this.TransparencyKey);
            //this.Opacity = 0.5;
        }

        private void MakeTransparent(bool on)
        {
            /*var initialStyle = GetWindowLong(this.Handle, -20);
            if (on)
                SetWindowLong(this.Handle, -20, initialStyle | 0x80000);
            else
                SetWindowLong(this.Handle, -20, initialStyle ^ 0x80000);*/
        }

        public void MakeNonInteractive(bool on)
        {
            if (ClickThrough == on)
                return;

            ClickThrough = on;

            var initialStyle = GetWindowLong(this.Handle, -20);
            if (on)
                SetWindowLong(this.Handle, -20, initialStyle | 0x20);
            else
                SetWindowLong(this.Handle, -20, initialStyle & (initialStyle ^ 0x20));
        }

        private bool IsApplicationChildForm(IntPtr handle)
        {
            if (handle == game.Itchy.Handle)
                return false;

            foreach (Form form in Application.OpenForms)
                if (form.Handle == handle)
                    return true;

            return false;
        }

        public void UpdateOverlay()
        {
            if (!this.ClickThrough && !this.propertiesExpandButton.Expanded)
                MakeNonInteractive(true);

            var foreGroundWindow = GetForegroundWindow();
            var newState = foreGroundWindow == this.game.Process.MainWindowHandle;
            if (newState != this.TopMost)
                this.TopMost = newState;

            if (!IsApplicationChildForm(foreGroundWindow) && !newState)
            {
                if (this.Visible)
                    this.Hide();
                return;
            }

            if (!this.Visible)
                this.Show();

            RECT rect;
            GetWindowRect(this.game.Process.MainWindowHandle, out rect);

            if (rect.Left == this.Location.X && rect.Right == this.Location.Y)
                return;

            this.SetDesktopLocation(rect.Left, rect.Top);
            this.Size = new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool IsIconic(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private void OverlayWindow_Load(object sender, EventArgs e)
        {
            //PostInit();

            //SetupControlPositions();
        }

        private void SetupControlPositions()
        {
            //RECT rect;
            //GetWindowRect(this.game.Process.MainWindowHandle, out rect);

            //translucentPanel1.Size = new Size(400, 40);
            //translucentPanel1.Location = new Point(115, rect.Height - translucentPanel1.Size.Height - 55);
            //itchyLabel.Font = Itchy.d2font;
        }

        private void OverlayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void OverlayWindow_Paint(object sender, PaintEventArgs e)
        {
            //var hb = new HatchBrush(HatchStyle.Percent90, this.TransparencyKey);
            //e.Graphics.FillRectangle(hb, this.DisplayRectangle);
        }

        private void SetupSettings(GameSettings settings)
        {
            lightHackCheckBox.Checked = settings.LightHack.Enabled;
            weatherHackCheckBox.Checked = settings.WeatherHack.Enabled;

            packetReceiveHackCheckBox.Checked = settings.ReceivePacketHack.Enabled;
            blockFlashCheckBox.Checked = settings.ReceivePacketHack.BlockFlash;
            fastTeleCheckBox.Checked = settings.ReceivePacketHack.FastTele;
            fastPortalCheckBox.Checked = settings.ReceivePacketHack.FastPortal;

            itemNameHackCheckBox.Checked = settings.ItemNameHack.Enabled;
            showEthCheckBox.Checked = settings.ItemNameHack.ShowEth;
            showItemLevelCheckBox.Checked = settings.ItemNameHack.ShowItemLevel;
            showItemPriceCheckBox.Checked = settings.ItemNameHack.ShowItemPrice;
            showRuneNumberCheckBox.Checked = settings.ItemNameHack.ShowRuneNumber;
            changeColorCheckBox.Checked = settings.ItemNameHack.ChangeItemColor;

            viewInventoryHackCheckBox.Checked = settings.ViewInventory.Enabled;
            viewInventoryKeybindButton.Key = settings.ViewInventory.ViewInventoryKey;

            infravisionHackCheckBox.Checked = settings.Infravision.Enabled;
            hideCorpsesCheckBox.Checked = settings.Infravision.HideCorpses;
            hideDyingCheckBox.Checked = settings.Infravision.HideDying;
            hideItemCheckBox.Checked = settings.Infravision.HideItems;

            revealActKeybindButton.Key = settings.RevealAct.Key;
            openStashKeybindButton.Key = settings.OpenStash.Key;
            openCubeKeybindButton.Key = settings.OpenCube.Key;
            fastExitKeybindButton.Key = settings.FastExit.Key;
            townPortalKeybindButton.Key = settings.FastPortal.Key;
            goToTownCheckBox.Checked = settings.FastPortal.GoToTown;

            enableChickenCheckBox.Checked = settings.Chicken.Enabled;
            chickenToTownCheckBox.Checked = settings.Chicken.ChickenToTown;
            chickenOnHostileTextBox.Checked = settings.Chicken.ChickenOnHostile;
            chickenLifePctTextBox.Text = settings.Chicken.ChickenLifePercent.ToString();
            chickenManaPctTextBox.Text = settings.Chicken.ChickenManaPercent.ToString();
        }

        private GameSettings GetSettings()
        {
            var settings = new GameSettings();

            settings.LightHack.Enabled = lightHackCheckBox.Checked;
            settings.WeatherHack.Enabled = weatherHackCheckBox.Checked;

            settings.ReceivePacketHack.Enabled = packetReceiveHackCheckBox.Checked;
            settings.ReceivePacketHack.BlockFlash = blockFlashCheckBox.Checked;
            settings.ReceivePacketHack.FastTele = fastTeleCheckBox.Checked;
            settings.ReceivePacketHack.FastPortal = fastPortalCheckBox.Checked;

            settings.ItemNameHack.Enabled = itemNameHackCheckBox.Checked;
            settings.ItemNameHack.ShowEth = showEthCheckBox.Checked;
            settings.ItemNameHack.ShowItemLevel = showItemLevelCheckBox.Checked;
            settings.ItemNameHack.ShowItemPrice = showItemPriceCheckBox.Checked;
            settings.ItemNameHack.ShowRuneNumber = showRuneNumberCheckBox.Checked;
            settings.ItemNameHack.ChangeItemColor = changeColorCheckBox.Checked;

            settings.ViewInventory.Enabled = viewInventoryHackCheckBox.Checked;
            settings.ViewInventory.ViewInventoryKey = viewInventoryKeybindButton.Key;

            settings.Infravision.Enabled = infravisionHackCheckBox.Checked;
            settings.Infravision.HideCorpses = hideCorpsesCheckBox.Checked;
            settings.Infravision.HideDying = hideDyingCheckBox.Checked;
            settings.Infravision.HideItems = hideItemCheckBox.Checked;

            settings.RevealAct.Key = revealActKeybindButton.Key;
            settings.OpenStash.Key = openStashKeybindButton.Key;
            settings.OpenCube.Key = openCubeKeybindButton.Key;
            settings.FastExit.Key = fastExitKeybindButton.Key;
            settings.FastPortal.Key = townPortalKeybindButton.Key;
            settings.FastPortal.GoToTown = goToTownCheckBox.Checked;

            settings.Chicken.Enabled = enableChickenCheckBox.Checked;
            settings.Chicken.ChickenToTown = chickenToTownCheckBox.Checked;
            settings.Chicken.ChickenOnHostile = chickenOnHostileTextBox.Checked;

            try
            {
                var val = Convert.ToDouble(chickenLifePctTextBox.Text);
                settings.Chicken.ChickenLifePercent = val;
            }
            catch (Exception) { }

            try
            {
                var val = Convert.ToDouble(chickenManaPctTextBox.Text);
                settings.Chicken.ChickenManaPercent = val;
            }
            catch (Exception) { }

            return settings;
        }

        private int GetHackCost()
        {
            int cost = 0;
            if (lightHackCheckBox.Checked)
                cost += HackSettings.Cost;
            if (weatherHackCheckBox.Checked)
                cost += HackSettings.Cost;
            if (packetReceiveHackCheckBox.Checked)
                cost += PacketReceivedHackSettings.Cost;
            if (itemNameHackCheckBox.Checked)
                cost += ItemNameHackSettings.Cost;
            if (viewInventoryHackCheckBox.Checked)
                cost += ViewInventorySettings.Cost;
            if (infravisionHackCheckBox.Checked)
                cost += InfravisionSettings.Cost;

            return cost;
        }

        private bool ValidateSettings()
        {
            return GetHackCost() <= 4;
        }

        private void packetReceiveHackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = packetReceiveHackCheckBox.Checked;

            blockFlashCheckBox.Enabled = state;
            fastTeleCheckBox.Enabled = state;
            fastPortalCheckBox.Enabled = state;

            goToTownCheckBox.Enabled = state;

            enableChickenCheckBox.Enabled = state;
            chickenToTownCheckBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenOnHostileTextBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenLifePctTextBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenManaPctTextBox.Enabled = state && enableChickenCheckBox.Checked;
        }

        private void itemNameHackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = itemNameHackCheckBox.Checked;

            showEthCheckBox.Enabled = state;
            showItemLevelCheckBox.Enabled = state;
            showItemPriceCheckBox.Enabled = state;
            showRuneNumberCheckBox.Enabled = state;
            showSocketsCheckBox.Enabled = state;
            changeColorCheckBox.Enabled = state;
        }

        private void viewInventoryHackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = viewInventoryHackCheckBox.Checked;

            viewInventoryKeybindButton.Enabled = state;
        }

        private void infravisionHackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = infravisionHackCheckBox.Checked;

            hideCorpsesCheckBox.Enabled = state;
            hideDyingCheckBox.Enabled = state;
            hideItemCheckBox.Enabled = state;
        }

        private void enableChickenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = enableChickenCheckBox.Checked;

            chickenToTownCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            chickenOnHostileTextBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            chickenLifePctTextBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            chickenManaPctTextBox.Enabled = state && packetReceiveHackCheckBox.Checked;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            SetupSettings(game.Settings);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            propertiesExpandButton.Expanded = false;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
            {
                MessageBox.Show(this, "Too many hacks selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            game.Settings = GetSettings();
            game.ApplySettings();
        }

        public void InGameStateChanged(bool inGame)
        {
            if (inGame)
            {
                logTranslucentPanel.Show();
                itchyLabel.Show();
                logExpandButton.Show();
            }
            else
            {
                logTranslucentPanel.Hide();
                itchyLabel.Hide();
                logExpandButton.Hide();
            }
        }

        public bool HandleMessage(Keys key, MessageEvent mEvent)
        {
            if (key == Keys.LControlKey || key == Keys.RControlKey)
                return true;

            var t = this.GetType();

            var changed = false;
            foreach (var f in t.GetFields())
            {
                if (f.FieldType != typeof(KeybindButton))
                    continue;

                var b = (KeybindButton)f.GetValue(this);
                if (b.WaitingKeyPress)
                {
                    changed = true;
                    if (key == Keys.Escape)
                        b.Reset();
                    else
                        b.Key = key;

                    foreach (var f2 in t.GetFields())
                    {
                        if (f2.FieldType != typeof(KeybindButton))
                            continue;

                        if (f2.Name == f.Name)
                            continue;

                        var b2 = (KeybindButton)f2.GetValue(this);
                        if (b2.Key == key)
                            b2.Key = Keys.None;
                    }
                }
            }

            return changed;
        }
    }
}
