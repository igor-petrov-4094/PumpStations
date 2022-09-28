using System;

namespace OmronControllerUdpService
{
    public static class Transform
    {
        public static ushort swapBytes(ushort x)
        {
            int b0 = x & 0xff;
            int b1 = (x >> 8) & 0xff;
            return (ushort)((b0 * 256) + b1);
        }

        // старший байт лежит по смещению +0, младший +1 (omron)
        public static ushort getUshortFromByteBuffer(ref byte[] byteArray, int offset)
        {
            int b0 = byteArray[offset];
            int b1 = byteArray[offset+1];

            return (ushort)((b0 * 256) + b1);
        }

        // старший байт лежит по смещению +0, младший +1 (omron)
        public static UInt32 getUInt32FromByteBuffer(ref byte[] byteArray, int offset)
        {
            int b0 = byteArray[offset];
            int b1 = byteArray[offset + 1];
            int b2 = byteArray[offset + 2];
            int b3 = byteArray[offset + 3];

            int res =
                    (b0 << 24) +
                    (b1 << 16) +
                    (b2 << 8) +
                    (b3);

            return (uint)res;
        }

        public static bool getBitFromWord(ushort value, byte bitNo)
        {
            int x = (value >> bitNo) & 1;
            return x != 0 ? true : false;
        }
    }
}
