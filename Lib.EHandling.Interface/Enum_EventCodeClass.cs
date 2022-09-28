using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EHandling.Interface
{
    /// <summary>
    /// Класс кода событий
    /// </summary>
    public enum Enum_EventCodeClass
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Успешное исполнение
        /// </summary>
        Success = 100,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning = 200,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 300,

        /// <summary>
        /// Авария
        /// </summary>
        Failure = 400,
    }
}
