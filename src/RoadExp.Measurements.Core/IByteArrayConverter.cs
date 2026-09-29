namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Преобразование "сырых" данных 
    /// в массив байт и обратно
    /// </summary>
    /// <typeparam name="T">Тип сырых данных</typeparam>
    public interface IByteArrayConverter<T>
    {
        /// <summary>
        /// Преобразует "сырые" данные, поступающие
        /// от измерительной системы в массив байт
        /// </summary>
        /// <param name="rawData">"Сырые" данные</param>
        /// <returns>Массив байт, соответствующий "сырым" данным</returns>
        byte[] ToByteArray(T rawData);

        /// <summary>
        /// Преобразует массив данных в "сырые" данные
        /// </summary>
        /// <param name="source"></param>
        /// <returns>"Сырые" данные</returns>
        T FromByteArray(byte[] source);
    }
}
