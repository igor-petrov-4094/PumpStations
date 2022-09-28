using System;
using System.Collections.Generic;
using System.Text;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Логирование на консоль
    /// </summary>
    public class Log_Console : Log_Base, ILog
    {
        public override void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            /*           
            В многопоточном приложении использование Console.ForegroundColor без исключительного доступа 
            приводит к ошибкам типа:

2021-04-27 18:18:13.4323 | ERROR | def | [Send/SET_MC_STATE/Ошибка при обработке объекта/кода][An error occurred while updating the entries. See the inner exception for details.]
или
2021-04-27 18:05:32.8635 | ERROR | def | [][Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.]           
            
            if (logOptions.HasFlag(LogOptions.TypeError))
                Console.ForegroundColor = ConsoleColor.Red;
            else if (logOptions.HasFlag(LogOptions.TypeWarning))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
            */

            if (logOptions.HasFlag(LogOptions.DoNotCarriageReturn))
                Console.Write(s);
            else
                Console.WriteLine(s);
        }
    }
}
