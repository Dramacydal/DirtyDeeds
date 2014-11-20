using WhiteMagic;

namespace DD.Breakpoints
{
    public class D2BreakPoint : HardwareBreakPoint
    {
        public D2Game Game { get; set; }
        public string ModuleName { get { return moduleName; } }

        protected string moduleName;

        public D2BreakPoint(D2Game game, string moduleName, int address)
            : base(address, 1, Condition.Code)
        {
            this.Game = game;
            this.moduleName = moduleName;
        }
    }
}
