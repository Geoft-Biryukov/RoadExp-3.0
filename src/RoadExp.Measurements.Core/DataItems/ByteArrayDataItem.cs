namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Представляет неизменяемый (immutable) контейнер бинарных данных, полученных от измерительной системы.
    /// </summary>
    public sealed class ByteArrayDataItem : IDataItem<byte[]>
    {        
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ByteArrayDataItem"/> заданными меткой времени и массивом байт.
        /// </summary>
        /// <param name="index">Метка времени (уникальный индекс) получения данных.</param>
        /// <param name="value">Массив байт, содержащий данные измерительной системы.</param>
        public ByteArrayDataItem(long index, byte[] value)
        {
            Index = index;
            Value = value;
        }     
        
        /// <summary>
        /// Получает метку времени (индекс) регистрации данных.
        /// </summary>
        public long Index { get; }

        /// <summary>
        /// Получает неизменяемую последовательность байт данных измерительной системы.
        /// </summary>
        public byte[] Value { get; }
    }
}
