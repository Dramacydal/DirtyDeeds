using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Win32HWBP;

namespace Itchy
{
    public partial class Itchy : Form
    {
        public Itchy()
        {
            InitializeComponent();
        }

        ProcessDebugger pd = null;
        Thread th = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (pd == null)
            {
                if (!WinApi.SetDebugPrivileges())
                {
                    MessageBox.Show("Failed to set debug privileges");
                    return;
                }

                Process[] list = Process.GetProcessesByName("d2loader-1.12");
                if (list.Length == 0)
                    return;

                var process = list[0];
                pd = new ProcessDebugger(process.Id);
                th = ProcessDebugger.Run(ref pd);

                MessageBox.Show("Attached");
            }
        }

        private void Itchy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pd != null)
            {
                pd.StopDebugging();
                th.Join();
            }
        }

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

        private void installButton_Click(object sender, EventArgs e)
        {
            if (pd == null)
                return;

            var bp = new LightBreakPoint(0x5F907, 1, HardwareBreakPoint.Condition.Code);
            pd.AddBreakPoint("D2Client.dll", bp);

            var bp2 = new RainBreakPoint(0x73A02, 1, HardwareBreakPoint.Condition.Code);
            pd.AddBreakPoint("d2common.dll", bp2);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (pd == null)
                return;

            pd.RemoveBreakPoints();
        }
    }
}
