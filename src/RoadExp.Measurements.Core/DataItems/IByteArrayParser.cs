namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет интерфейс парсера для преобразования массивов байт в типизированные объекты данных.
    /// </summary>
    /// <typeparam name="T">Тип результирующих данных</typeparam>
    public interface IByteArrayParser<out T> where T : IDataItem
    {
        /// <summary>
        /// Преобразует исходный массив байт измерительной системы в объект целевого типа.
        /// </summary>
        /// <param name="data">Контейнер с исходными бинарными данными.</param>
        /// <returns>Экземпляр типа <typeparamref name="T"/>, содержащий обработанные данные.</returns>
        T Parse(in MeasurementBatch batch);
    }
}
