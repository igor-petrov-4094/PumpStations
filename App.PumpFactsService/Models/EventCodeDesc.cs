using Lib.EHandling.Interface;

namespace App.PumpFactsService.Models
{
    public static class EventCodeDesc_PumpFactsService
    {
        public static EventCodeSystem codeSystem = new EventCodeSystem("CodeSystem.PumpFactsService");

        public static EventCodeDescriptor CellAddressParseError = new EventCodeDescriptor(
            codeSystem,
            1000,
            "Ошибка разбора адреса ячейки",
            "Ошибка разбора адреса ячейки",
             Enum_EventCodeClass.Error
        );

        public static EventCodeDescriptor NoPacketNumberValueError = new EventCodeDescriptor(
            codeSystem,
            1001,
            "Нет поля 'Номер пакета'",
            "Нет поля 'Номер пакета'",
             Enum_EventCodeClass.Error
        );

        public static EventCodeDescriptor InvalidParameterDescriptor = new EventCodeDescriptor(
            codeSystem,
            1002,
            "Неверный дескриптор параметра",
            "Неверный дескриптор параметра",
             Enum_EventCodeClass.Error
        );
    }
}
