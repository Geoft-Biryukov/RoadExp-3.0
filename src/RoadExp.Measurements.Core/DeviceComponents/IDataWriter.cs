using RoadExp.Measurements.Core.Serialization;

namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет низкоуровневый контракт для записи элементов данных измерительной системы в файл.
    /// </summary>    
    public interface IDataWriter : IDisposable
    {
        /// <summary>
        /// Создает или открывает целевой файл для записи данных, записывая в него заголовки и метаданные.
        /// </summary>
        /// <param name="fileName">Путь к файлу или имя создаваемого файла.</param>
        /// <param name="fileHeader">Общий заголовок файла с метаданными измерительной сессии.</param>
        /// <param name="metadata">Дополнительный массив байт с пользовательскими или системными метаданными.</param>
        void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata);

        /// <summary>
        /// Записывает элемент данных во внутренний буфер или непосредственно в файл.
        /// </summary>
        /// <param name="batch">Элемент измеренных данных для записи.</param>
        void Write(in MeasurementBatch batch);

        /// <summary>
        /// Принудительно сбрасывает все буферизированные данные из памяти на физический диск.
        /// </summary>
        void Flush();
    }

}
