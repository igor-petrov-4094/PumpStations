using System;
using System.Collections.Generic;
using System.Text;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Логирование в несколько логов
    /// </summary>
    public class Log_MultiLog : Log_Base, ILog
    {
        List<ILog> ilogs = new List<ILog>();

        public Log_MultiLog(params ILog[] logs)
        {
            foreach (var log in logs)
                registerLog(log);
        }

        public void registerLog(ILog logToRegister)
        {
            ilogs.Add(logToRegister);
        }

        public override void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            foreach (ILog ilog in ilogs)
                ilog._log(s, logOptions, eventCodeDescriptor);
        }
    }
}