using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public class ParameterValue
    {
        /// <summary>
        /// Переменная типа int для значений
        /// </summary>
        private int _intValue;

        /// <summary>
        /// Переменная типа bool для значений
        /// </summary>
        private bool _boolValue;

        /// <summary>
        /// Дескриптор параметра
        /// </summary>
        public  ParameterDescriptor parameterDescriptor { get; private set; }

        /// <summary>
        /// Время последней модификации
        /// </summary>
        public DateTime? lastModified { get; private set; } = null;

        /// <summary>
        /// _shortValue как свойство
        /// </summary>
        public int intValue
        {
            get => _intValue;
            set { _intValue = value; lastModified = DateTime.Now; }
        }

        /// <summary>
        /// _boolValue как свойство
        /// </summary>
        public bool boolValue 
        { 
            get => _boolValue;
            set { _boolValue = value; lastModified = DateTime.Now; }
        }

        /// <summary>
        /// Значение инициализировано?
        /// </summary>
        public bool isInitialized => lastModified != null;

        /// <summary>
        /// Логический тип данных?
        /// </summary>
        public bool isBoolean { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_parameterDescriptor"></param>
        public ParameterValue(ParameterDescriptor _parameterDescriptor)
        {
            this.parameterDescriptor = _parameterDescriptor;
        }

        public void getValueFromOther(ParameterValue other)
        {
            this.boolValue = other.boolValue;
            this.intValue = other.intValue;
        }
    }
}
