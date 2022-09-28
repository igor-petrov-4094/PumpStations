using System;

namespace Lib.Log.Interface
{
    /// <summary>
    /// Флаги логирования
    /// </summary>
    [Flags]
    public enum LogOptions : long
    {
        None = 0,

        /// <summary>
        /// Не производить перенос строки
        /// </summary>
        DoNotCarriageReturn = 1,

        /// <summary>
        /// Не логировать стек трассировки при ошибке
        /// </summary>
        DoNotLogStackTraceOnError = 2,
    }
}
