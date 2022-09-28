using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    /// <summary>
    /// Объект для обмена данными плюс информация о результате выполнения плюс сообщение об ошибке
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DTOWithResultInfo<T> where T: class
    {
        /// <summary>
        /// Успешно или нет
        /// </summary>
        public bool ok { get; private set; }

        /// <summary>
        /// Расширенный код ответа
        /// </summary>
        public int extendedCode { get; private set; }

        /// <summary>
        /// Описание ответа
        /// </summary>
        public string description { get; private set; }

        /// <summary>
        /// Значение ответа
        /// </summary>
        public T? resultValue { get; private set; }

        private DTOWithResultInfo(bool ok, int extendedCode, string description, T? resultValue)
        {
            this.ok = ok;
            this.extendedCode = extendedCode;
            this.description = description;
            this.resultValue = resultValue;
        }

        /// <summary>
        /// Создать "ошибочный" ответ из исключения
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="extendedCode"></param>
        /// <returns></returns>
        public static DTOWithResultInfo<T> createFromException(Exception ex, int extendedCode = 0)
        {
            return new DTOWithResultInfo<T>(false, extendedCode, ex.Message, null);
        }

        /// <summary>
        /// Создать успешный ответ
        /// </summary>
        /// <param name="goodResultValue"></param>
        /// <param name="extendedCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DTOWithResultInfo<T> createNormal(T? goodResultValue, int extendedCode = 0, string message = "")
        {
            return new DTOWithResultInfo<T>(true, extendedCode, message, goodResultValue);
        }
    }
}
