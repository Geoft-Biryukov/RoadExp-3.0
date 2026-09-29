using RoadExp.Measurements.Core.Serialization;

namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет контракт для компонента сохранения и потоковой записи измеренных данных в файл.
    /// </summary>
    public interface IDataStorage : IAsyncDisposable
    {
        /// <summary>
        /// Инициализирует (открывает) целевой файл для последующей записи данных.
        /// </summary>
        /// <param name="fileName">Путь к файлу или имя создаваемого файла.</param>
        /// <param name="fileHeader">Общий заголовок файла с метаданными измерительной сессии.</param>
        /// <param name="metadata">Дополнительный массив байт с пользовательскими или системными метаданными.</param>
        void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata);

        /// <summary>
        /// Запускает фоновый процесс асинхронной записи поступающих данных на диск.
        /// </summary>
        void StartSaving();

        /// <summary>
        /// Приостанавливает или завершает процесс записи данных на диск, сохраняя дескриптор файла открытым.
        /// </summary>
        void StopSaving();

        /// <summary>
        /// Добавляет элемент данных в очередь на сохранение.
        /// </summary>
        /// <param name="item">Элемент измеренных данных для записи.</param>
        void Add(in MeasurementBatch batch);
    }

}
