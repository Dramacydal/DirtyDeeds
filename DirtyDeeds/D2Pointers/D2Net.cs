using WhiteMagic;
using WhiteMagic.Modules;

namespace DD.D2Pointers
{
    public class D2Net : ModulePointer   // 0x6FBF0000
    {
        public D2Net(int offset)
            : base("D2Net.dll", offset)
        { }

        public static D2Net ReceivePacket = new D2Net(0x6FBF64A0 - 0x6FBF0000);  // stdcall (BYTE *aPacket, DWORD aLen)
        public static D2Net SendPacket = new D2Net(0x6F20); // stdcall (size_t aLen, DWORD arg1, BYTE* aPacket)
    }
}
