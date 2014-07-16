using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
