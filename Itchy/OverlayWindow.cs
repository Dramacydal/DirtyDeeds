using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Itchy
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }

    public partial class OverlayWindow : Form
    {
        public D2Game game { get; set; }

        protected Thread syncThread;

        public volatile bool disposing = false;
        public bool ClickThrough { get; set; }

        public OverlayWindow()
        {
            InitializeComponent();
        }

        protected void PostInit()
        {
            MakeInteractive(false);

            //graphics = this.CreateGraphics();
            //graphics.SetClip(new Rectangle(0, 0, 1600, 1200));
            syncThread = new Thread(() => WindowChecker(this));
            syncThread.Start();
        }

        public void MakeInteractive(bool on)
        {
            if (ClickThrough == !on)
                return;

            ClickThrough = !on;

            var initialStyle = GetWindowLong(this.Handle, -20);
            if (on)
                SetWindowLong(this.Handle, -20, initialStyle ^ (0x80000 | 0x20));
            else
                SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
        }

        protected static void WindowChecker(OverlayWindow w)
        {
            while (true)
            {
                try
                {
                    w.Invoke((MethodInvoker)delegate { if (!w.disposing) w.RelocateWindow(); });
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }
                Thread.Sleep(300);
            }
        }

        private void RelocateWindow()
        {
            this.TopMost = GetForegroundWindow() == this.game.Process.MainWindowHandle;
            if (!this.TopMost)
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private void OverlayWindow_Load(object sender, EventArgs e)
        {
            PostInit();
        }

        enum MessageEvent : int
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_HOTKEY = 0x312,
        }

        public bool HandleMessage(int code, IntPtr wParam, IntPtr lParam)
        {
            var mEvent = (MessageEvent)wParam.ToInt32();
            var vkCode = (VKeyCodes)Marshal.ReadInt32(lParam);

            Console.WriteLine(mEvent.ToString() + " " + vkCode.ToString());

            return true;
        }

        private void OverlayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
