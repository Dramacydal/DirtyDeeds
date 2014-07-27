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

namespace Itchy
{
    public partial class OverlayWindow : Form
    {
        public D2Game game { get; set; }

        protected Thread syncThread;
        protected Thread gameCheckThread;

        public volatile bool disposing = false;
        public bool ClickThrough { get; set; }

        public OverlayWindow()
        {
            InitializeComponent();
        }

        //
        protected void PostInit()
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            MakeTransparent(true);
            MakeNonInteractive(true);

            syncThread = new Thread(() => WindowChecker(this));
            syncThread.Start();

            gameCheckThread = new Thread(() => GameChecker(this));
            gameCheckThread.Start();

            CheckInGame(true);

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

        protected static void WindowChecker(OverlayWindow w)
        {
            while (true)
            {
                try
                {
                    w.Invoke((MethodInvoker)delegate { if (!w.disposing) w.RelocateWindow(); });
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.ToString());
                }
                Thread.Sleep(300);
            }
        }

        protected static void GameChecker(OverlayWindow w)
        {
            while (true)
            {
                try
                {
                    w.Invoke((MethodInvoker)delegate { w.CheckInGame(); });
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.ToString());
                }
                Thread.Sleep(300);
            }
        }

        volatile bool inGame;
        private void CheckInGame(bool first = false)
        {
            var check = game.GetPlayerUnit() != 0;
            if (check == inGame && !first)
                return;

            if (check)
            {
                var name = game.GetPlayerName();
                game.PlayerName = name;
            }
            else
                game.PlayerName = "";
            game.Itchy.games.ResetBindings();

            inGame = check;

            if (inGame)
            {
                button1.Show();
                logTranslucentPanel.Show();
                itchyLabel.Show();
                logExpandButton.Show();

                game.EnteredGame();
            }
            else
            {
                button1.Hide();
                logTranslucentPanel.Hide();
                itchyLabel.Hide();
                logExpandButton.Hide();

                game.ExitedGame();
            }
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

        private void RelocateWindow()
        {
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
            PostInit();

            SetupControlPositions();
        }

        private void SetupControlPositions()
        {
            RECT rect;
            GetWindowRect(this.game.Process.MainWindowHandle, out rect);

            //translucentPanel1.Size = new Size(400, 40);
            //translucentPanel1.Location = new Point(115, rect.Height - translucentPanel1.Size.Height - 55);
            itchyLabel.Font = Itchy.d2font;
        }

        private void OverlayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void OverlayWindow_Paint(object sender, PaintEventArgs e)
        {
            //var hb = new HatchBrush(HatchStyle.Percent90, this.TransparencyKey);
            //e.Graphics.FillRectangle(hb, this.DisplayRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form1 = new Form1();
            form1.Show();
        }
    }
}
