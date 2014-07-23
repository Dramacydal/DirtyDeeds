using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Itchy
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
            var itchy = new Itchy();
            Application.AddMessageFilter(new MyMessageFilter(itchy));
            Application.Run(itchy);
        }
    }

    public class MyMessageFilter : IMessageFilter
    {
        Itchy itchy;

        public MyMessageFilter(Itchy itchy) { this.itchy = itchy; }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x312:
                    //MessageBox.Show(m.ToString());
                    //Console.WriteLine(s
                    break;
                //case 0x101:
                  //  MessageBox.Show("KeyUp " + m.ToString());
                    //break;
            }

            //return true;
            return false;
            return !itchy.HandleMessage(ref m);
        }
    }
}
