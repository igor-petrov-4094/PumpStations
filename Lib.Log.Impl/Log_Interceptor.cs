using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    public delegate void LogInterceptor(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor);

    /// <summary>
    /// Перехватчик логирования
    /// </summary>
    public class Log_Interceptor : Log_Base, ILog
    {
        public static event LogInterceptor logInterceptor = (a, b, c) =>
        {

        };

        public override void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            logInterceptor?.Invoke(s, logOptions, eventCodeDescriptor);
        }
    }
}
