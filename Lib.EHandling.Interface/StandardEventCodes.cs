using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EHandling.Interface
{
    /// <summary>
    /// Объекты стандартных событий
    /// </summary>
    public static class StandardEventCodes
    {
        public static EventCodeSystem eventCodeSystem = new EventCodeSystem(
            _name: "Общая система кодов",
            _textId: "CMN"
        );

        public static EventCodeDescriptor Info = new EventCodeDescriptor(
            _eventCode: 0,
            _eventCodeClass: Enum_EventCodeClass.Unknown,
            _eventCodeSystem: eventCodeSystem,
            _longName: "Информация",
            _shortName: "Инф."
        );

        public static EventCodeDescriptor Ok = new EventCodeDescriptor(
            _eventCode: 1,
            _eventCodeClass: Enum_EventCodeClass.Success,
            _eventCodeSystem: eventCodeSystem,
            _longName: "OK",
            _shortName: "OK"
        );

        public static EventCodeDescriptor Error = new EventCodeDescriptor(
            _eventCode: -1,
            _eventCodeClass: Enum_EventCodeClass.Error,
            _eventCodeSystem: eventCodeSystem,
            _longName: "Общая ошибка",
            _shortName: "Общ. ошиб."
        );

        public static EventCodeDescriptor Warn = new EventCodeDescriptor(
            _eventCode: -2,
            _eventCodeClass: Enum_EventCodeClass.Warning,
            _eventCodeSystem: eventCodeSystem,
            _longName: "Общее предупреждение",
            _shortName: "Общ. предупр."
        );
    }
}
