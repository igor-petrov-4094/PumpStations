using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.NoDepsUtils
{
    public static class Extensions_Exception
    {
        /// <summary>
        /// Возвращает из исключения сообщение со стеком
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string extension_Message(this Exception ex, bool doLogStackTrace = true)
        {
            int index = 1;
            Exception temp = ex;
            StringBuilder sb = new StringBuilder($"({index}):[{temp.Message}]"); // не заменять на _Message()

            sb.AppendLine();

            if (doLogStackTrace)
            {
                sb.Append("<< stack trace: ");
                sb.AppendLine();
                sb.Append(ex.StackTrace);
                sb.Append(">>");
            }

            while (true)
            {
                if (temp.InnerException == null || index >= 100)
                    break;
                index++;
                temp = temp.InnerException;
                sb.Append($"({index}):[{temp.Message}]"); // не заменять на _Message()
            }
            return sb.ToString();
        }
    }
}
