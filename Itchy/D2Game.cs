using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Win32HWBP;

namespace Itchy
{
    public class D2Game
    {
        public Process Process { get { return process; } }
        public ProcessDebugger Debugger { get { return pd; } }
        public Thread Thread { get { return th; } }
        public bool Installed { get { return pd != null; } }

        protected Process process;
        protected ProcessDebugger pd;
        protected Thread th;

        public D2Game(Process process)
        {
            this.process = process;
        }

        public override string ToString()
        {
            return process.MainModule.FileVersionInfo.InternalName + " - " + process.Id + (Installed ? " (*)" : "");
        }

        public bool Exists()
        {
            process.Refresh();

            if (process.HasExited)
                return false;

            return true;
        }

        public bool Install()
        {
            process.Refresh();
            if (process.HasExited)
                return false;

            try
            {
                pd = new ProcessDebugger(process.Id);
                th = ProcessDebugger.Run(ref pd);

                if (!pd.WaitForComeUp(500))
                    return false;

                // 0x6FD80C92 0x30C92
                var bp2 = new RainBreakPoint(0x30C92, 1, HardwareBreakPoint.Condition.Code);
                pd.AddBreakPoint("d2common.dll", bp2);

                // 0x6FAD33A7 0x233A7
                var bp = new LightBreakPoint(0x233A7, 1, HardwareBreakPoint.Condition.Code);
                pd.AddBreakPoint("d2client.dll", bp);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public bool Detach()
        {
            process.Refresh();
            if (process.HasExited)
                return true;

            try
            {
                pd.StopDebugging();
                th.Join();
                pd = null;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool GetPlayerUnit(out UnitAny unit)
        {
            var addr = pd.BlackMagic.AllocateMemory(1024);

            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + 0x613C0);
            pd.BlackMagic.Asm.AddLine("retn");
            var playerAddr = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);
            if (playerAddr == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = (UnitAny)pd.BlackMagic.ReadObject(playerAddr, typeof(UnitAny));
            return true;
        }

        public void Test()
        {
            var addr = pd.BlackMagic.AllocateMemory(1024);

            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + 0x613C0);
            pd.BlackMagic.Asm.AddLine("retn");
            var playerAddr = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);

            UnitAny unit;
            if (GetPlayerUnit(out unit))
                MessageBox.Show(unit.dwAct.ToString());
        }
    }
}
