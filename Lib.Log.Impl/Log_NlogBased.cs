using System;
using System.Collections.Generic;
using System.Text;

using NLog;
using NLog.LayoutRenderers;
using NLog.Config;

using Lib.Log.Interface;
using Lib.EHandling.Interface;

namespace Lib.Log.Impl
{
    [LayoutRenderer("ecode")]
    public class HelloWorldLayoutRenderer : LayoutRenderer
    {
        public static int errorCode;

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append($"E{errorCode:D6}"); // дополняется до 6-ти знаков
        }
    }

    /// <summary>
    /// NLog-based лог
    /// </summary>
    public class Log_NlogBased : Log_Base, ILog
    {
        private readonly ILogger nlog;

        public Log_NlogBased(
            string producer,
            string app,
            string profileNo,
            string suffix,
            string nlogConfigFilePath,
            string? loggerName = null
        )
        {
            LayoutRenderer.Register<HelloWorldLayoutRenderer>("ecode"); //generic

            LogManager.ThrowExceptions = false;
            LogManager.Configuration = new XmlLoggingConfiguration(nlogConfigFilePath);

            LogManager.Configuration.Variables["xsuffix"] = suffix;
            LogManager.Configuration.Variables["xproducer"] = producer;
            LogManager.Configuration.Variables["xapp"] = app;
            LogManager.Configuration.Variables["xsuffix"] = suffix;
            LogManager.Configuration.Variables["xprofileno"] = profileNo;
            LogManager.ReconfigExistingLoggers();

            nlog = LogManager.GetLogger(loggerName ?? "default");
        }

        public override void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor)
        {
            lock (this)
            {
                int savedErrorCode = HelloWorldLayoutRenderer.errorCode;
                HelloWorldLayoutRenderer.errorCode = (int)eventCodeDescriptor.eventCode;
                try
                {
                    switch (eventCodeDescriptor.eventCodeClass)
                    {
                        case Enum_EventCodeClass.Unknown: nlog.Info(s); break;
                        case Enum_EventCodeClass.Success: nlog.Info(s); break;
                        case Enum_EventCodeClass.Warning: nlog.Warn(s); break;
                        case Enum_EventCodeClass.Error: nlog.Error(s); break;
                        default: nlog.Info(s); break;
                    }
                }
                finally
                {
                    HelloWorldLayoutRenderer.errorCode = savedErrorCode;
                }
            }
        }
    }
}
