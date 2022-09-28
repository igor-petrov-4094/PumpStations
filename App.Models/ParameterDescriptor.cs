using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    /// <summary>
    /// Параметр контроллера
    /// </summary>
    public class ParameterDescriptor
    {
        /// <summary>
        /// Строковый идентификатор
        /// </summary>
        public string parameterStringId { get; set; } = "";

        /// <summary>
        /// Читаемое имя
        /// </summary>
        public string readableName { get; set; } = "";

        /// <summary>
        /// Является техническим
        /// </summary>
        public bool isTech { get; set; } = false;

        /// <summary>
        /// Формат для отображения
        /// </summary>
        public string format { get; set; } = "";

        /// <summary>
        /// Адрес ячейки
        /// </summary>
        public string cellAddress { get; set; } = "";

        /// <summary>
        /// Множитель реального значения
        /// </summary>
        public float realValueFactor { get; set; } = 1.0F;

        /// <summary>
        /// Выражение для вычисления через другие параметры
        /// </summary>
        public string expression { get; set; } = "";

        /// <summary>
        /// Номер пакета
        /// </summary>
        public int? packetNo { get; set; } = null;

        /// <summary>
        /// Вычисляемый параметр?
        /// </summary>
        public bool isComputable { get { return !string.IsNullOrEmpty(expression); } }

        /// <summary>
        /// Имя группы
        /// </summary>
        public string groupName_runTime;

        /// <summary>
        /// Имя аварии
        /// </summary>
        public string failureName { get; set; }
    }
}
