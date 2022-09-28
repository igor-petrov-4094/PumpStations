using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.EHandling.Interface;
using Lib.Log.Interface;

namespace Lib.Log.Impl
{
    /// <summary>
    /// Логирование в пустоту
    /// </summary>
    public class Log_Null : ILog
    {
        public object? getLogSetting(string settingName)
        {
            return null;
        }

        public void setLogSetting(string settingName, object settingValue)
        {
        }

        public void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
        }

        public void _log(string s, Exception ex, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
        }
    }
}
