namespace RoadExp.Measurements.Core.Serialization
{
    /// <summary>
    /// Фабрика создания заголовков файлов измерительных систем
    /// </summary>
    public interface IFileHeaderFactory
    {
        /// <summary>
        /// Создает заголовок файла конкретной измерительной системы
        /// </summary>
        /// <param name="systemId">ID конкретной измерительной системы</param>
        /// <param name="metadataSize">Размер (в байтах) метаданных конкретной измерительной системы</param>
        /// <returns>Заполненный заголовок файла конкретной измерительной системы</returns>
        CommonFileHeader CreateFileHeader(ushort systemId, ushort metadataSize);
    }
}
