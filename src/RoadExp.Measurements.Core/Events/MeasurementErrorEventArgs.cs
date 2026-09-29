namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Аргументы события ошибки измерительной системы
    /// Класс ErrorEventArgs есть в System.IO. Чтобы избежать конфликтов имён 
    /// добавлен префикс Measurement
    /// </summary>
    public class MeasurementErrorEventArgs : EventArgs
    {
        #region constructors
        public MeasurementErrorEventArgs(string message, Exception? exception, bool isCritical)
        {
            Message = message;
            Exception = exception;
            IsCritical = isCritical;
        }

        public MeasurementErrorEventArgs(string message, Exception? exception)
            : this(message, exception, false)
        { }

        public MeasurementErrorEventArgs(string message, bool isCritical)
            : this(message, null, isCritical)
        { }

        public MeasurementErrorEventArgs(string message)
           : this(message, null, false)
        { }
        #endregion

        #region properties
        /// <summary>
        /// Текст сообщения об ошибке
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Перехваченное исключение
        /// </summary>
        public Exception? Exception { get; }
        
        /// <summary>
        /// Является ли ошибка критической
        /// </summary>
        public bool IsCritical { get; }
        #endregion
    }
}
