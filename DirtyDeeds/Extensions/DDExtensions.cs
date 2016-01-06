using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace DD.Extensions
{
    public static class DDExtensions
    {
        public static T Read<T>(this MemoryHandler m, uint addr) where T : struct
        {
            return m.Read<T>(new IntPtr(addr));
        }

        public static ushort ReadUShort(this MemoryHandler m, uint addr)
        {
            return m.ReadUShort(new IntPtr(addr));
        }

        public static short ReadShort(this MemoryHandler m, uint addr)
        {
            return m.ReadShort(new IntPtr(addr));
        }

        public static uint ReadUInt(this MemoryHandler m, uint addr)
        {
            return m.ReadUInt(new IntPtr(addr));
        }

        public static byte ReadByte(this MemoryHandler m, uint addr)
        {
            return m.ReadByte(new IntPtr(addr));
        }

        public static string ReadUTF16String(this MemoryHandler m, uint addr)
        {
            return m.ReadUTF16String(new IntPtr(addr));
        }

        public static void WriteUTF16String(this MemoryHandler m, uint addr, string str, bool nullTerminated = true)
        {
            m.WriteUTF16String(new IntPtr(addr), str, nullTerminated);
        }

        public static byte[] ReadBytes(this MemoryHandler m, uint addr, int count)
        {
            return m.ReadBytes(new IntPtr(addr), count);
        }

        public static void WriteByte(this MemoryHandler m, uint addr, byte value)
        {
            m.WriteByte(new IntPtr(addr), value);
        }

        public static void Write<T>(this MemoryHandler m, uint addr, T value)
        {
            m.Write<T>(new IntPtr(addr), value);
        }

        public static T[] ReadArray<T>(this MemoryHandler m, uint addr, int count)
        {
            return m.ReadArray<T>(new IntPtr(addr), count);
        }

        /*public static uint Call(this MemoryHandler m, uint addr, MagicConvention cv, params object[] args)
        {
            return m.Call(new IntPtr(addr), cv, args);
        }*/
    }
}
