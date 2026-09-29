namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Неизменяемая структура, представляющая пакет данных
    /// </summary>
    public readonly struct MeasurementBatch
    {
        private readonly byte[] _dataArray;

        public MeasurementBatch(long index, byte[] dataArray)
        {
            Index = index;
            _dataArray = dataArray;
        }

        /// <summary>
        /// Получает временную метку пакета.
        /// </summary>
        public long Index { get; }

        /// <summary>
        /// Получает массив, содержащий сырые данные измерительной системы
        /// </summary>
        public byte[] Data => _dataArray;

    }

}
