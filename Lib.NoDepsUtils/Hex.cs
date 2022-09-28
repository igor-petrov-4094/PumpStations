using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.NoDepsUtils
{
    public static class Hex
    {
        public static string ByteArrayToHexString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", "");
        }

        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string extension_ByteArrayToHexString(this byte[] byteArray)
        {
            return ByteArrayToHexString(byteArray);
        }
    }
}
