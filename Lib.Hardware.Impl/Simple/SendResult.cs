using System;

namespace OmronControllerUdpService
{
    /// <summary>
    /// Результат отправки команды
    /// </summary>
    public struct SendResult
    {
        /// <summary>
        /// Успешно или нет
        /// </summary>
        public bool result;

        /// <summary>
        /// Полученные данные
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string message;

        /// <summary>
        /// Код команды
        /// </summary>
        public ushort commandCode;

        /// <summary>
        /// Код результата
        /// </summary>
        public ushort endCode;

        /// <summary>
        /// Возвращает полный результат
        /// </summary>
        public bool goodResult
        {
            get
            {
                return result && (endCode == 0);
            }
        }

        /// <summary>
        /// Выбрасывает исключени, если есть ошибка
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void throwExceptionIfError()
        {
            if (!goodResult)
                throw new Exception(message);
        }
    }

    /// <summary>
    /// Результат отправки команды (типизированный)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SendResultTyped<T>
    {
        /// <summary>
        /// SendResult
        /// </summary>
        public SendResult result;

        /// <summary>
        /// Значение
        /// </summary>
        public T value;

        public SendResultTyped(SendResult _result, T _value)
        {
            value = _value;
            result = _result;
        }

        /// <summary>
        /// Выбрасывает исключение, если есть ошибка
        /// </summary>
        public void throwExceptionIfError()
        {
            result.throwExceptionIfError();
        }
    }
}
