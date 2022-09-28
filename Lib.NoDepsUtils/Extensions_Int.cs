using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.NoDepsUtils
{
    public static class Extensions_Int
    {
        /// <summary>
        /// Возвращает младшую 16-битную часть 32-битного целого
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ushort extension_Low(this uint x)
        {
            return (ushort)(x & 65535);
        }

        /// <summary>
        /// Возвращает старшую 16-битную часть 32-битного целого
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ushort extension_High(this uint x)
        {
            return ((ushort)((x >> 16) & 65535));
        }
    }
}
