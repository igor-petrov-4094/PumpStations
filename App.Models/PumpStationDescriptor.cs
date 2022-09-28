using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    /// <summary>
    /// Дескриптор станции
    /// </summary>
    public class PumpStationDescriptor
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
        /// IP адрес
        /// </summary>
        public string ipAddress { get; set; } = "";

        /// <summary>
        /// Порт
        /// </summary>
        public short port { get; set; } = 9600;

        /// <summary>
        /// Местонахождение
        /// </summary>
        public string location { get; set; } = "";

        /// <summary>
        /// Используемая схема
        /// </summary>
        public string schemaName { get; set; } = "";

        /// <summary>
        /// Коэффициент тока двигателя (на разных станциях принят разный)
        /// </summary>
        public float motorCurrentFactor { get; set; } = 1.0F;
    }
}
