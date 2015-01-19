using WhiteMagic;
using WhiteMagic.Modules;

namespace DD.D2Pointers
{
    public class Game : ModulePointer   // 0x400000
    {
        public Game(int offset)
            : base("Game.exe", offset)
        { }

        public static Game mainThread = new Game(0x401227 - 0x400000);
    }
}
