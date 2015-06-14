using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Game.Breakpoints
{
    // 1.13 0x997B2 0x6FB497B2
    public class ViewInventoryBp1 : D2BreakPoint
    {
        public ViewInventoryBp1(D2Game game) : base(game, "d2client.dll", 0x997B2) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Esi = Game.GetViewingUnit();
            ctx.Eip += 6;

            return true;
        }
    }
}
