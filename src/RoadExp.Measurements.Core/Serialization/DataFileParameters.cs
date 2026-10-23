namespace RoadExp.Measurements.Core.Serialization
{
    /// <summary>
    /// Содержит информацию о файле данных измерительной системы
    /// </summary>
    public class DataFileParameters
    {
        public DataFileParameters(string fileName, CommonFileHeader header, byte[] metadata)
        {
            FileName = fileName;
            FileHeader = header;
            Metadata = metadata;
        }

        /// <summary>
        /// Путь к файлу или имя файла 
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Общий заголовок файла
        /// </summary>
        public CommonFileHeader FileHeader { get; }
        
        /// <summary>
        /// Метаданные измерительной системы
        /// </summary>
        public byte[] Metadata { get; }
    }
}
