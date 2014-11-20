using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
{
    // 1.13 0x98E84 0x6FB48E84
    public class ViewInventoryBp2 : D2BreakPoint
    {
        public ViewInventoryBp2(D2Game game) : base(game, "d2client.dll", 0x98E84) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Ebx = Game.GetViewingUnit();
            ctx.Eip += 6;

            return true;
        }
    }
}
