using WhiteMagic;

namespace DD.D2Pointers
{
    public class Fog : ModulePointer     // 0x6FF50000
    {
        public Fog(uint offset)
            : base("Fog.dll", offset)
        { }

        public static Fog gdwBitMasks = new Fog(0x6FF77020 - 0x6FF50000);
    }
}
