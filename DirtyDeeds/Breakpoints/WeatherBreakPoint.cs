using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
{
    public class WeatherBreakPoint : D2BreakPoint
    {
        public WeatherBreakPoint(D2Game game) : base(game, "d2common.dll", 0x30C92) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Al &= 0;
            ctx.Eip += 4;

            return true;
        }
    }
}
