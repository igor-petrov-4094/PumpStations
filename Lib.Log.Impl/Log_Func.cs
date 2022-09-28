using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Логирование в функцию
    /// </summary>
    public class Log_Func : Log_Base, ILog
    {
        public delegate void LogFunction(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor);

        private readonly LogFunction logFunction;

        public Log_Func(LogFunction _logFunction)
        {
            logFunction = _logFunction;
        }

        public override void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            logFunction?.Invoke(s, logOptions, eventCodeDescriptor);
        }
    }
}
