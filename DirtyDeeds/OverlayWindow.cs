using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DirtyDeedsControls;
using DD.Game.Settings;
using DD.Extensions;
using DD.Tools;
using DD.Game;
using DD.Game.Enums;

namespace DD
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
            if (handle == game.DirtyDeeds.Handle)
                return false;

            foreach (Form form in Application.OpenForms)
                if (form.Handle == handle)
                    return true;

            return false;
        }

        public void UpdateOverlay()
        {
            if (!this.ClickThrough)
                MakeNonInteractive(true);

            var foreGroundWindow = GetForegroundWindow();
            if (foreGroundWindow == this.game.Process.MainWindowHandle)
            {
                if (!this.Visible)
                    this.Show();
            }
            else if (foreGroundWindow != this.Handle)
            {
                if (this.Visible)
                    this.Hide();
                return;
            }

            RECT rect;
            GetWindowRect(this.game.Process.MainWindowHandle, out rect);

            if (rect.Left - 300 == this.Location.X && rect.Top - 300 == this.Location.Y)
                return;

            this.Location = new Point(rect.Left - 300, rect.Top - 300);
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

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
            statsExpandButton.SetDraggable(true);
            statsRefreshButton.SetDraggable(true);
            mobInfoButton.SetDraggable(true);

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
            //dirtyDeedsLabel.Font = DirtyDeeds.d2font;
        }

        private void OverlayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.OverlaySettings.LogFontSize = logTextBox.Font.Size;
            game.OverlaySettings.LogPosition = logHolder.Location;
            game.OverlaySettings.StatsPosition = statsHolder.Location;
        }

        private void OverlayWindow_Paint(object sender, PaintEventArgs e)
        {
            //var hb = new HatchBrush(HatchStyle.Percent90, this.TransparencyKey);
            //e.Graphics.FillRectangle(hb, this.DisplayRectangle);
        }

        public void InGameStateChanged(bool inGame)
        {
            if (inGame)
            {
                dirtyDeedsLabel.Show();
                logHolder.Show();
                statsHolder.Show();
            }
            else
            {
                dirtyDeedsLabel.Hide();
                logHolder.Hide();
                statsHolder.Hide();
                statsTextBox.Clear();
            }
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

            SetForegroundWindow(game.Process.MainWindowHandle);
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
            SetForegroundWindow(game.Process.MainWindowHandle);
        }

        private void logExpandButton_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(game.Process.MainWindowHandle);
            logClearButton.Visible = logExpandButton.Expanded;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
        }
    }
}
