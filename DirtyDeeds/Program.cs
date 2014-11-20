using System;
using System.Windows.Forms;

namespace DD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var dd = new DirtyDeeds();
            Application.Run(dd);
        }
    }
}
