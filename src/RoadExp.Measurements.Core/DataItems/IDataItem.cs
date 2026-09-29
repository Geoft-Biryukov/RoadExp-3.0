namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет базовый контракт для элемента данных, полученного от измерительной системы.
    /// </summary>
    public interface IDataItem
    {
        /// <summary>
        /// Получает временную метку регистрации данных.
        /// </summary>
        long Index { get; }
    }

    /// <summary>
    /// Определяет строго типизированный контракт для элемента измеренных данных.
    /// </summary>
    /// <typeparam name="T">Тип содержащихся данных измерительной системы.</typeparam>
    public interface IDataItem<out T> : IDataItem
    {
        /// <summary>
        /// Получает значение (полезную нагрузку) измеренных данных.
        /// </summary>
        T Value { get; }
    }
}
