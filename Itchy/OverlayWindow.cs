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

        protected Form logBackForm;

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
            if (!this.ClickThrough && !this.settingsExpandButton.Expanded)
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

            if (rect.Left - 300 == this.Location.X && rect.Top - 300 == this.Location.Y)
                return;

            //Console.WriteLine("{0} {1} {2} {3}", rect.Left - 300, this.Location.X, rect.Top - 300, this.Location.Y);
            //this.SetDesktopLocation(rect.Left - 300, rect.Top - 300);
            this.Location = new Point(rect.Left - 300, rect.Top - 300);
            //this.Size = new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
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
            SetupControlPositions();
        }

        private void SetupControlPositions()
        {
            logExpandButton.SetDraggable(true);
            settingsExpandButton.SetDraggable(true);
            statsExpandButton.SetDraggable(true);
            statsRefreshButton.SetDraggable(true);

            if (!game.OverlaySettings.SettingsPosition.IsEmpty)
                settingsHolder.Location = game.OverlaySettings.SettingsPosition;
            if (!game.OverlaySettings.LogPosition.IsEmpty)
                logHolder.Location = game.OverlaySettings.LogPosition;
            if (!game.OverlaySettings.StatsPosition.IsEmpty)
                statsHolder.Location = game.OverlaySettings.StatsPosition;
            if (game.OverlaySettings.LogFontSize != 0.0f)
                logTextBox.Font = new Font(logTextBox.Font.FontFamily, game.OverlaySettings.LogFontSize, FontStyle.Bold);

            //RECT rect;
            //GetWindowRect(this.game.Process.MainWindowHandle, out rect);

            //translucentPanel1.Size = new Size(400, 40);
            //translucentPanel1.Location = new Point(115, rect.Height - translucentPanel1.Size.Height - 55);
            //itchyLabel.Font = Itchy.d2font;
        }

        private void OverlayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.OverlaySettings.LogFontSize = logTextBox.Font.Size;
            game.OverlaySettings.SettingsPosition = settingsHolder.Location;
            game.OverlaySettings.LogPosition = logHolder.Location;
            game.OverlaySettings.StatsPosition = statsHolder.Location;
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
            itemTrackerCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.Enabled;
            enablePickitCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.EnablePickit;
            useTelekinesisCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.UseTelekinesis;
            enableTelepickCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.EnableTelepick;
            teleBackCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.TeleBack;
            pickInTownCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.TownPick;
            logRunesCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogRunes;
            logSetsCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogSets;
            logUniquesCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogUniques;
            logItemsCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogItems;
            resetPickitKeyBindButton.Key = settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key;

            itemNameHackCheckBox.Checked = settings.ItemNameHack.Enabled;
            showEthCheckBox.Checked = settings.ItemNameHack.ShowEth;
            showItemLevelCheckBox.Checked = settings.ItemNameHack.ShowItemLevel;
            showItemPriceCheckBox.Checked = settings.ItemNameHack.ShowItemPrice;
            showRuneNumberCheckBox.Checked = settings.ItemNameHack.ShowRuneNumber;
            showSocketsCheckBox.Checked = settings.ItemNameHack.ShowSockets;
            showItemCodeTextBox.Checked = settings.ItemNameHack.ShowItemCode;
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
            settings.ReceivePacketHack.ItemTracker.Enabled = itemTrackerCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.EnablePickit = enablePickitCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.UseTelekinesis = useTelekinesisCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.EnableTelepick = enableTelepickCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.TownPick = pickInTownCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.TeleBack = teleBackCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogRunes = logRunesCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogSets = logSetsCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogUniques = logUniquesCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogItems = logItemsCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key = resetPickitKeyBindButton.Key;

            settings.ItemNameHack.Enabled = itemNameHackCheckBox.Checked;
            settings.ItemNameHack.ShowEth = showEthCheckBox.Checked;
            settings.ItemNameHack.ShowItemLevel = showItemLevelCheckBox.Checked;
            settings.ItemNameHack.ShowItemPrice = showItemPriceCheckBox.Checked;
            settings.ItemNameHack.ShowRuneNumber = showRuneNumberCheckBox.Checked;
            settings.ItemNameHack.ShowSockets = showSocketsCheckBox.Checked;
            settings.ItemNameHack.ShowItemCode = showItemCodeTextBox.Checked;
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
            itemTrackerCheckBox.Enabled = state;

            goToTownCheckBox.Enabled = state;

            enableChickenCheckBox.Enabled = state;
            chickenToTownCheckBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenOnHostileTextBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenLifePctTextBox.Enabled = state && enableChickenCheckBox.Checked;
            chickenManaPctTextBox.Enabled = state && enableChickenCheckBox.Checked;

            logRunesCheckBox.Enabled = state && itemTrackerCheckBox.Checked;
            logSetsCheckBox.Enabled = state && itemTrackerCheckBox.Checked;
            logUniquesCheckBox.Enabled = state && itemTrackerCheckBox.Checked;
            logItemsCheckBox.Enabled = state && itemTrackerCheckBox.Checked;
            enablePickitCheckBox.Enabled = state && itemTrackerCheckBox.Checked;
            useTelekinesisCheckBox.Enabled = state && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked;
            enableTelepickCheckBox.Enabled = state && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked;
            teleBackCheckBox.Enabled = state && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked && enableTelepickCheckBox.Checked;
            pickInTownCheckBox.Enabled = state && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked;
            resetPickitKeyBindButton.Enabled = state && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked;
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
            showItemCodeTextBox.Enabled = state;
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


        private void itemNotificationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = itemTrackerCheckBox.Checked;

            logRunesCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            logSetsCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            logUniquesCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            logItemsCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            enablePickitCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked;
            useTelekinesisCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && enablePickitCheckBox.Checked;
            enableTelepickCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && enablePickitCheckBox.Checked;
            pickInTownCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && enablePickitCheckBox.Checked;
            teleBackCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && enablePickitCheckBox.Checked && enableTelepickCheckBox.Checked;
            resetPickitKeyBindButton.Enabled = state && packetReceiveHackCheckBox.Checked && enablePickitCheckBox.Checked;
        }

        private void enablePickitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = enablePickitCheckBox.Checked;

            useTelekinesisCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked;
            enableTelepickCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked;
            pickInTownCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked;
            teleBackCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked && enableTelepickCheckBox.Checked;
            resetPickitKeyBindButton.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked;
        }

        private void enableTelepickCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = enableTelepickCheckBox.Checked;

            teleBackCheckBox.Enabled = state && packetReceiveHackCheckBox.Checked && itemTrackerCheckBox.Checked && enablePickitCheckBox.Checked;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            SetupSettings(game.Settings);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            settingsExpandButton.Expanded = false;
            settingsExpandButton_Click(sender, e);
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
                itchyLabel.Show();
                logHolder.Show();
                statsHolder.Show();
            }
            else
            {
                itchyLabel.Hide();
                logHolder.Hide();
                statsHolder.Hide();
                statsTextBox.Clear();
            }
        }

        public bool HandleMessage(Keys key, MessageEvent mEvent)
        {
            if (this.ClickThrough)
                return true;

            var t = this.GetType();

            var changed = false;
            foreach (var f in t.GetFields())
            {
                if (f.FieldType != typeof(KeybindButton))
                    continue;

                var b = f.GetValue(this) as KeybindButton;
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

                        var b2 = f2.GetValue(this) as KeybindButton;
                        if (b2.Key == key)
                            b2.Key = Keys.None;
                    }
                }
            }

            return !changed;
        }

        private void reloadItemIniButton_Click(object sender, EventArgs e)
        {
            game.ItemProcessingSettings.Load();
        }

        protected override void OnClick(EventArgs e) { }
        protected override void OnMouseEnter(EventArgs e) { }
        protected override void OnMouseLeave(EventArgs e) { }
        protected override void OnMouseDoubleClick(MouseEventArgs e) { }
        protected override void OnMouseHover(EventArgs e) { }
        protected override void OnMouseMove(MouseEventArgs e) { }
        protected override void OnMouseDown(MouseEventArgs e) { }
        protected override void OnMouseUp(MouseEventArgs e) { }

        private void statsExpandButton_Click(object sender, EventArgs e)
        {
            if (statsExpandButton.Expanded)
            {
                BuildStats();
                statsRefreshButton.Show();
            }
            else
                statsRefreshButton.Hide();
        }

        private void settingsExpandButton_Click(object sender, EventArgs e)
        {
            if (settingsExpandButton.Expanded)
                settingsHolder.Size = new Size(620, 568);
            else
                settingsHolder.Size = new Size(20, 20);
        }

        protected void BuildStats()
        {
            if (!game.InGame)
                return;

            using (var suspender = new GameSuspender(game))
            {
                var pUnit = game.GetPlayerUnit();
                if (pUnit == 0 || !game.GameReady())
                    return;

                var log = statsTextBox;
                log.Clear();

                var negresByDiff = new int[] { 0, 40, 100 };

                log.AppendText("Str: {0} + {1} = {2}", Color.Empty, game.GetBaseUnitStat(pUnit, StatType.Strength),
                    game.GetUnitStat(pUnit, StatType.Strength) - game.GetBaseUnitStat(pUnit, StatType.Strength),
                    game.GetUnitStat(pUnit, StatType.Strength));

                log.AppendLine("Dex: {0} + {1} = {2}", Color.Empty, game.GetBaseUnitStat(pUnit, StatType.Dexterity),
                    game.GetUnitStat(pUnit, StatType.Dexterity) - game.GetBaseUnitStat(pUnit, StatType.Dexterity),
                    game.GetUnitStat(pUnit, StatType.Dexterity));

                log.AppendLine("Vit: {0} + {1} = {2}", Color.Empty, game.GetBaseUnitStat(pUnit, StatType.Vitality),
                    game.GetUnitStat(pUnit, StatType.Vitality) - game.GetBaseUnitStat(pUnit, StatType.Vitality),
                    game.GetUnitStat(pUnit, StatType.Vitality));

                log.AppendLine("Eng: {0} + {1} = {2}", Color.Empty, game.GetBaseUnitStat(pUnit, StatType.Energy),
                    game.GetUnitStat(pUnit, StatType.Energy) - game.GetBaseUnitStat(pUnit, StatType.Energy),
                    game.GetUnitStat(pUnit, StatType.Energy));

                log.AppendLine("", Color.Empty);

                log.AppendLine("Fire Resist: ", Color.Red);
                log.AppendText("{0} - {1} = {2} / {3}", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.FireResist),
                    negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.FireResist) - negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MaxFireResist) + 75);

                log.AppendLine("Cold Resist: ", Color.SkyBlue);
                log.AppendText("{0} - {1} = {2} / {3}", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.ColdResist),
                    negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.ColdResist) - negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MaxColdResist) + 75);

                log.AppendLine("Lightning Resist: ", Color.Yellow);
                log.AppendText("{0} - {1} = {2} / {3}", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.LightResist),
                    negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.LightResist) - negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MaxLightningResist) + 75);

                log.AppendLine("Poison Resist: ", Color.LawnGreen);
                log.AppendText("{0} - {1} = {2} / {3}", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.PoisonResist),
                    negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.PoisonResist) - negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MaxPoisonResist) + 75);

                log.AppendLine("Magic Resist: ", Color.Orange);
                log.AppendText("{0} - {1} = {2} / {3}", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.MagicResist),
                    negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MagicResist) - negresByDiff[game.GetDifficulty()],
                    game.GetUnitStat(pUnit, StatType.MaxMagicResist) + 75);

                log.AppendLine("Damage Resist: ", Color.SandyBrown);
                log.AppendText("{0}%", Color.Empty,
                    game.GetUnitStat(pUnit, StatType.DamageResist));

                log.AppendLine("", Color.Empty);

                log.AppendLine("Absorbs: ", Color.Empty);
                log.AppendText("{0}", Color.Red, game.GetUnitStat(pUnit, StatType.AbsorbFire));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}", Color.SkyBlue, game.GetUnitStat(pUnit, StatType.AbsorbCold));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}", Color.Yellow, game.GetUnitStat(pUnit, StatType.AbsorbLight));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}", Color.Orange, game.GetUnitStat(pUnit, StatType.AbsorbMagic));

                log.AppendLine("Absorb Pcts: ", Color.Empty);
                log.AppendText("{0}%", Color.Red, game.GetUnitStat(pUnit, StatType.AbsorbFirePercent));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}%", Color.SkyBlue, game.GetUnitStat(pUnit, StatType.AbsorbColdPercent));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}%", Color.Yellow, game.GetUnitStat(pUnit, StatType.AbsorbLightingPercent));
                log.AppendText("/", Color.Empty);
                log.AppendText("{0}%", Color.Orange, game.GetUnitStat(pUnit, StatType.AbsorbLightingPercent));

                log.AppendLine("", Color.Empty);

                log.AppendLine("Magic Find: ", Color.SkyBlue);
                log.AppendText("{0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.MagicFind));
                log.AppendLine("Gold Find: ", Color.Yellow);
                log.AppendText("{0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.GoldFind));

                log.AppendLine("", Color.Empty);

                log.AppendLine("Faster Cast Rate: {0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.FasterCastRate));
                log.AppendLine("Faster Hit Recovery: {0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.FasterHitRecovery));
                log.AppendLine("Increased Attack Speed: {0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.FasterAttackRate));
                log.AppendLine("Faster Block Rate: {0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.FasterBlockRate));
                log.AppendLine("Faster Run/Walk: {0}%", Color.Empty, game.GetUnitStat(pUnit, StatType.FasterMoveVelocity));

                log.AppendLine("", Color.Empty);

                log.AppendLine("Crushing Blow: {0}", Color.Empty, game.GetUnitStat(pUnit, StatType.CrushingBlow));
                log.AppendLine("Deadly Strike: {0}", Color.Empty, game.GetUnitStat(pUnit, StatType.DeadlyStrike));
            }
        }

        private void statsRefreshButton_Click(object sender, EventArgs e)
        {
            BuildStats();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.Test();
        }
    }
}
