namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Предоставляет данные для событий, возникающих при получении нового пакета измерений от измерительной системы.
    /// </summary>
    /// <typeparam name="T">Тип содержащихся измерительных данных, реализующий интерфейс <see cref="IDataItem"/>.</typeparam>
    public class DataEventArgs<T> : EventArgs where T : IDataItem
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DataEventArgs{T}"/> заданным элементом данных.
        /// </summary>
        /// <param name="data">Элемент измеренных данных, передаваемый вместе с событием.</param>
        public DataEventArgs(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Получает элемент данных, зарегистрированный измерительной системой.
        /// </summary>
        public T Data { get; }
    }
}
