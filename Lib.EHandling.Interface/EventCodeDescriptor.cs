using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EHandling.Interface
{
    /// <summary>
    /// Описатель кода события
    /// </summary>
    public class EventCodeDescriptor
    {
        /// <summary>
        /// Система кода события
        /// </summary>
        public readonly EventCodeSystem eventCodeSystem;

        /// <summary>
        /// Числовой код события
        /// </summary>
        public readonly int eventCode;

        /// <summary>
        /// Длинное имя события
        /// </summary>
        public readonly string longName;

        /// <summary>
        /// Короткое имя события
        /// </summary>
        public readonly string shortName;

        /// <summary>
        /// Класс события
        /// </summary>
        public readonly Enum_EventCodeClass eventCodeClass;

        public EventCodeDescriptor(
            EventCodeSystem _eventCodeSystem,
            int _eventCode,
            string _longName,
            string _shortName,
            Enum_EventCodeClass _eventCodeClass
        )
        {
            eventCodeSystem = _eventCodeSystem;
            eventCode = _eventCode;
            longName = _longName;
            shortName = _shortName;
            eventCodeClass = _eventCodeClass;
        }
    }
}
