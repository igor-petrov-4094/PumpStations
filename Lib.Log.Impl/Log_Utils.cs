using System;
using System.Collections.Generic;
using System.Text;

using Lib.NoDepsUtils;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Утилиты логирования
    /// </summary>
    public static class Log_Utils
    {
        /// <summary>
        /// Форматируем исключение
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string formatExceptionMessage(string s, Exception ex, bool doLogStackTrace = true)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('[');
            sb.Append(s);
            sb.Append(']');

            sb.Append('[');
            sb.Append(ex.extension_Message(doLogStackTrace)); // текст исключения + стек
            sb.Append(']');

            return sb.ToString();
        }
    }
}
