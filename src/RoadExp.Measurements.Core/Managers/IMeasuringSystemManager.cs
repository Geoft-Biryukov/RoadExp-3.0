
namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет контракт менеджера, отвечающего за жизненный цикл, конфигурацию и оперативное управление конкретной измерительной системой.
    /// </summary>
    public interface IMeasuringSystemManager : IDisposable
    {
        /// <summary>
        /// Получает уникальный строковый идентификатор измерительной системы.
        /// </summary>
        string SystemId { get; }

        /// <summary>
        /// Получает управляемый экземпляр измерительной системы.
        /// </summary>
        IMeasuringSystem MeasuringSystem { get; }

        /// <summary>
        /// Получает текущие конфигурационные настройки измерительной системы.
        /// </summary>
        MeasuringSystemSettings Settings { get; }

        /// <summary>
        /// Получает или задает значение, указывающее, переведена ли система в режим эмуляции (виртуального устройства).
        /// </summary>
        bool IsEmulationMode { get; set; }

        /// <summary>
        /// Обновляет параметры и конфигурацию измерительной системы на основе измененных настроек.
        /// </summary>
        void UpdateParameters();

        /// <summary>
        /// Асинхронно инициализирует управляемую измерительную систему и подготавливает её к работе.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Деинициализирует измерительную систему, останавливая сбор данных и освобождая занятые ресурсы.
        /// </summary>
        void Deinitialize();

        /// <summary>
        /// Инициализирует целевой файл по указанному пути и запускает процесс непрерывного сбора и записи данных.
        /// </summary>
        /// <param name="filePath">Полный путь к файлу для сохранения измерительных данных.</param>
        void StartAcquisition(string filePath);

        /// <summary>
        /// Останавливает процесс сбора данных и завершает запись в файл.
        /// </summary>
        void StopAcquisition();
    }
}
