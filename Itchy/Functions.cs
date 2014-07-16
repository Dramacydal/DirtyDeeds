using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Win32HWBP;

namespace Itchy
{
    class LightBreakPoint : HardwareBreakPoint
    {
        public LightBreakPoint(int address, uint len, Condition condition) : base(address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax = 0xFF;     // light density
            ctx.Eip += 0xEB;    // skip code

            return true;
        }
    }

    class RainBreakPoint : HardwareBreakPoint
    {
        public RainBreakPoint(int address, uint len, Condition condition) : base(address, len, condition) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax &= 0xFFFFFF00;
            ctx.Eip += 4;

            return true;
        }
    }

    public partial class Itchy : Form
    {
        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWow64Process([In] IntPtr processHandle,
        //     [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        public D2Game SelectedGame { get { return (D2Game)clientsComboBox.SelectedItem; } }
        public D2Game[] Games { get { return clientsComboBox.Items.Cast<D2Game>().ToArray(); } }

        BindingList<D2Game> games = new BindingList<D2Game>();

        protected void UpdateGames()
        {
            for (var i = 0; i < games.Count; ++i)
            {
                D2Game g = games[i];
                if (!g.Exists())
                {
                    games.RemoveAt(i);
                    i = 0;
                }
            }

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
                            games.Add(new D2Game(process));
                    }
                }
                catch (System.NullReferenceException)
                {
                }
                catch (System.ComponentModel.Win32Exception)
                {
                }
            }
        }
    }
}
