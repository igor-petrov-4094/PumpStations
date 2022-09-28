using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Lib.EHandling.Interface;

namespace Lib.Log.Interface
{
    /// <summary>
    /// Интерфейс логирования
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Стандартная процедура логирования
        /// </summary>
        /// <param name="s"></param>
        /// <param name="logOptions"></param>
        /// <param name="eventCodeDescriptor"></param>
        void _log(string s, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor);

        /// <summary>
        /// Стандартная процедура логирования исключения
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ex"></param>
        /// <param name="logOptions"></param>
        /// <param name="eventCodeDescriptor"></param>
        void _log(string s, Exception ex, LogOptions logOptions, EventCodeDescriptor eventCodeDescriptor);

        /// <summary>
        /// Получить настройки логирования
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        object? getLogSetting(string settingName);

        /// <summary>
        /// Установить настройки логирования
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="settingValue"></param>
        void setLogSetting(string settingName, object settingValue);

        // эти новые методы (ниже) должны стоять здесь. это как-то соотносится с реализацией в Log_Base.
        // если перенести их выше, выходит ошибка типа "метод setLogSetting не реализован в Log_Console".
        // вероятно, это ошибка в C#.

        /// <summary>
        /// Информация
        /// </summary>
        /// <param name="s"></param>
        /// <param name="eventCodeDescriptor"></param>
        /// <param name="logOptions"></param>
        void Info(string s, EventCodeDescriptor? eventCodeDescriptor = null, LogOptions logOptions = LogOptions.None)
        {
            _log(s, logOptions, eventCodeDescriptor ?? StandardEventCodes.Info);
        }

        /// <summary>
        /// Предупреждение
        /// </summary>
        /// <param name="s"></param>
        /// <param name="eventCodeDescriptor"></param>
        /// <param name="logOptions"></param>
        void Warn(string s, EventCodeDescriptor? eventCodeDescriptor = null, LogOptions logOptions = LogOptions.None)
        {
            _log(s, logOptions, eventCodeDescriptor ?? StandardEventCodes.Warn);
        }

        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="s"></param>
        /// <param name="eventCodeDescriptor"></param>
        /// <param name="logOptions"></param>
        void Error(string s, EventCodeDescriptor? eventCodeDescriptor = null, LogOptions logOptions = LogOptions.None)
        {
            _log(s, logOptions, eventCodeDescriptor ?? StandardEventCodes.Error);
        }

        /// <summary>
        /// Ошибка (из исключения)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ex"></param>
        /// <param name="eventCodeDescriptor"></param>
        /// <param name="logOptions"></param>
        void Error(string s, Exception ex, EventCodeDescriptor? eventCodeDescriptor = null, LogOptions logOptions = LogOptions.None)
        {
            _log(s, ex, logOptions, eventCodeDescriptor ?? StandardEventCodes.Error);
        }
    }
}
