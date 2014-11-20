using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
{
    public class LightBreakPoint : D2BreakPoint
    {
        public LightBreakPoint(D2Game game) : base(game, "d2client.dll", 0x233A7) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Eax = 0xFF;     // light density
            ctx.Eip += 0xEB;    // skip code

            return true;
        }
    }
}
