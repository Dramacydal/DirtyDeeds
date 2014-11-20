using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
{
    // 1.13 0x97E3F 0x6FB47E3F
    // new 0x97E41 0x6FB47E41
    // 97E1A 6FB47E1A
    public class ViewInventoryBp3 : D2BreakPoint
    {
        public ViewInventoryBp3(D2Game game) : base(game, "d2client.dll", 0x97E1A) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            var viewingUnit = Game.GetViewingUnit();
            //var val = pd.ReadUInt(ctx.Edi);
            //if (val != 1)
            ctx.Edi = viewingUnit;

            //ctx.Ecx = pd.ReadUInt(ctx.Edi + 0x60);
            //ctx.Eip += 3;

            ctx.Eip += 7;

            return true;
        }
    }
}
