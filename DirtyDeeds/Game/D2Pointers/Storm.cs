using WhiteMagic.Modules;

namespace DD.Game.D2Pointers
{
    public class Storm : ModulePointer   // 0x6FBF0000
    {
        public Storm(int offset)
            : base("Storm.dll", offset)
        { }

        public static Storm pHandle = new Storm(0x6FC42A50 - 0x6FBF0000);
    }
}
