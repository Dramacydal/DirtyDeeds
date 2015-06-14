using System;
using WhiteMagic;
using WhiteMagic.WinAPI.Types;

namespace DD.Game.Breakpoints
{
    public class D2BreakPoint : HardwareBreakPoint
    {
        public D2Game Game { get; set; }
        public string ModuleName { get { return moduleName; } }

        protected string moduleName;

        public D2BreakPoint(D2Game game, string moduleName, uint address)
            : base(new IntPtr(address), 1, BreakpointCondition.Code)
        {
            this.Game = game;
            this.moduleName = moduleName;
        }
    }
}
