using System;

namespace DD.Tools
{
    public class ByteConverter
    {
        public static int GetBits(byte[] bytes, ref int offset, int length)
        {
            offset += length;
            return GetBits(bytes, offset - length, length);
        }

        public static int GetBits(byte[] bytes, int offset, int length)
        {
            int bytesLen = bytes.Length * 8;
            if (offset < 0 || !(offset < bytesLen))
                throw new ArgumentOutOfRangeException("offset");
            if (length < 1 || length > 32 || (offset + length > bytesLen))
                throw new ArgumentOutOfRangeException("length");

            int result = 0;
            int bytePos = offset / 8;
            int bitPos = offset % 8;
            byte b = bytes[bytePos];
            int byteBits;
            int totBits = 0;

            while (length > 0)
            {
                if (bitPos == 8)
                {
                    b = bytes[++bytePos];
                    bitPos = 0;
                }
                byteBits = Math.Min(length, 8 - bitPos);
                result |= GetBits(b, bitPos, byteBits) << totBits;
                bitPos += byteBits;
                totBits += byteBits;
                length -= byteBits;
            }
            return result;
        }

        public static int GetBits(byte val, int offset, int length)
        {
            //TODO: support BE: if (bigEndian) offset = 8 - offset - length;
            return ((val & (((1 << (offset + length)) - 1) & ~((1 << offset) - 1))) >> offset);
        }
    }
}
