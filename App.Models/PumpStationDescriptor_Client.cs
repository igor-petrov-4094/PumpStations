using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    /// <summary>
    /// Дескриптор станции (базовая информация)
    /// </summary>
    public class PumpStationDescriptor_Client
    {
        /// <summary>
        /// Читаемое имя
        /// </summary>
        public string readableName { get; set; } = "";

        /// <summary>
        /// Строковый идентификатор
        /// </summary>
        public string stringId { get; set; } = "";

        /// <summary>
        /// Время последней доступности станции
        /// </summary>
        public DateTime? lastAvailableTime { get; set; } = null;

        /// <summary>
        /// Аварии (через запятую)
        /// </summary>
        public string failures { get; set; }

        /// <summary>
        /// Коэффициент тока двигателя (на разных станциях принят разный)
        /// </summary>
        public float motorCurrentFactor { get; set; } = 1.0F;
    }
}
