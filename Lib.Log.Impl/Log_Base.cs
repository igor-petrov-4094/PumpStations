using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Tracing;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Базовый абстрактный класс для логирования
    /// </summary>
    public abstract class Log_Base : ILog
    {
        public virtual object? getLogSetting(string settingName)
        {
            return null;
        }

        public virtual void setLogSetting(string settingName, object settingValue)
        {
        }

        public abstract void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor);

        public virtual void _log(string s, Exception ex, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            bool doLogStackTrace = logOptions.HasFlag(LogOptions.DoNotLogStackTraceOnError) == false;

            _log(
                Log_Utils.formatExceptionMessage(s, ex, doLogStackTrace), 
                logOptions, 
                eventCodeDescriptor
            );
        }
    }
}
