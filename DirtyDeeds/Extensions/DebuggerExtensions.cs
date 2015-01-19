using WhiteMagic;
using WhiteMagic.Modules;

namespace DD.Extensions
{
    public static class DebuggerExtensions
    {
        public static uint ReadUInt(this ProcessDebugger pd, ModulePointer offs)
        {
            return pd.ReadUInt(pd.GetAddress(offs));
        }

        public static byte ReadByte(this ProcessDebugger pd, ModulePointer offs)
        {
            return pd.ReadByte(pd.GetAddress(offs));
        }
    }
}
