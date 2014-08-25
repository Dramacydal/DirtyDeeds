using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ItchyControls;
using WhiteMagic;

namespace Itchy
{
    public partial class Itchy : Form
    {
        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWow64Process([In] IntPtr processHandle,
        //     [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        //public D2Game SelectedGame { get { return (D2Game)clientsComboBox.SelectedItem; } }
        public List<D2Game> Games { get { return games; } }

        public List<D2Game> games = new List<D2Game>();

        protected void UpdateGames()
        {
            games.ForEach((g) =>
                {
                    if (!g.Exists())
                        g.Dispose();
                });
            games.RemoveAll(g => !g.Exists());

            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    //bool retVal = false;
                    //if (!IsWow64Process(process.Handle, out retVal))
                    //    continue;

                    //if (retVal)
                    //    continue;
                    //if (!process.ProcessName.Contains("d2"))
                    //     continue;

                    /*if (process.HasExited)
                        continue;*/

                    if (process.MainModule.FileVersionInfo.InternalName.ToLower().Contains("diablo ii"))
                    {
                        bool found = false;
                        foreach (var g in games)
                        {
                            if (g.Process.Id == process.Id)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            games.Add(new D2Game(process, this));
                    }
                }
                catch (Exception) { }
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

            var attachItem = new D2ToolstripMenuItem(D2ToolstripType.AttachToAll);
            attachItem.Click += (sender, e) =>
                {
                    foreach (var obj in clientsToolStripMenuItem.DropDownItems)
                    {
                        if (obj.GetType() != typeof(D2ToolstripMenuItem))
                            continue;

                        var item = (D2ToolstripMenuItem)obj;
                        if (item.Type != D2ToolstripType.Game)
                            continue;

                        item.Attach();
                    }
                };

            var detachItem = new D2ToolstripMenuItem(D2ToolstripType.DetachFromAll);
            detachItem.Click += (sender, e) =>
                {
                    foreach (var obj in clientsToolStripMenuItem.DropDownItems)
                    {
                        if (obj.GetType() != typeof(D2ToolstripMenuItem))
                            continue;

                        var item = (D2ToolstripMenuItem)obj;
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
                var t = new D2ToolstripMenuItem(g);
                t.MouseDown += (sender, e) =>
                    {
                        needCloseToolstrip = false;
                    };

                clientsToolStripMenuItem.DropDownItems.Add(t);
            }
        }
    }
}
