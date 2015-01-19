using WhiteMagic;
using WhiteMagic.Modules;

namespace DD.D2Pointers
{
    public class Fog : ModulePointer     // 0x6FF50000
    {
        public Fog(int offset)
            : base("Fog.dll", offset)
        { }

        public static Fog gdwBitMasks = new Fog(0x6FF77020 - 0x6FF50000);
    }
}
