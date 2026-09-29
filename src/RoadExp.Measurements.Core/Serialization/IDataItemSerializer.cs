namespace RoadExp.Measurements.Core.Serialization
{
    /// <summary>
    /// Поддержка сериализации IDataItem в массив байтов
    /// </summary>
    /// <typeparam name="T">Тип сериализуемых данных</typeparam>
    public interface IDataItemSerializer<T> where T : IDataItem
    {
        /// <summary>
        /// Выполняет сериализацию в массив байтов
        /// </summary>
        /// <param name="item">Сериализуемые данные</param>
        /// <returns>Массив байтов, соответствующий item</returns>
        byte[] Serialize(T item);

        /// <summary>
        /// Выполняет десериализацию из массива байт
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        T Deserialize(byte[] source);
    }
}
