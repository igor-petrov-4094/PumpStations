using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    /// <summary>
    /// Значение параметра (для сервиса)
    /// </summary>
    public class ParameterValue_Client
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
        /// Переменная типа UInt16 для значений
        /// </summary>
        public int intValue { get; set; }

        /// <summary>
        /// Переменная типа bool для значений
        /// </summary>
        public bool boolValue { get; set; }

        /// <summary>
        /// Инициализирован?
        /// </summary>
        public bool isInitialized { get; set; }

        /// <summary>
        /// Логический тип данных?
        /// </summary>
        public bool isBoolean { get; set; }

        /// <summary>
        /// Коэффициент значения
        /// </summary>
        public float realValueFactor { get; set; }
    }
}
