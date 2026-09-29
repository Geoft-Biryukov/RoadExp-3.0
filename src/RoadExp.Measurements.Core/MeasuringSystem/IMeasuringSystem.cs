namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет базовый контракт для управления измерительной системой, координации сбора данных и сохранения результатов.
    /// </summary>
    public interface IMeasuringSystem : IMeasurementErrorProvider, IDisposable
    {
        /// <summary>
        /// Получает значение, указывающее, запущена ли в данный момент сессия сбора данных.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Получает текущие конфигурационные настройки измерительной системы.      
        /// </summary>
        MeasuringSystemSettings Settings { get; }

        /// <summary>
        /// Инициализирует измерительную систему и все связанные компоненты для подготовки к работе.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Переводит измерительную систему в исходное состояние, отключая сбор данных и освобождая ресурсы.
        /// </summary>
        void Deinitialize();

        /// <summary>
        /// Открывает или обновляет целевой файл для записи измерительных данных на основе текущих настроек.
        /// </summary>
        void OpenFile();

        /// <summary>
        /// Запускает непрерывный процесс сбора данных и их параллельную запись на диск.
        /// </summary>
        void StartAcquisition();

        /// <summary>
        /// Останавливает процесс сбора и записи данных, завершая текущую сессию измерений.
        /// </summary>
        void StopAcquisition();

        /// <summary>
        /// Пересоздает и обновляет внутренний адаптер связи устройства в соответствии с изменившимися настройками.
        /// </summary>     
        void UpdateAdapter();
    }

    /// <summary>
    /// Определяет строго типизированный контракт измерительной системы, предоставляющий доступ к потоку данных.
    /// </summary>
    /// <typeparam name="T">Тип регистрируемых измерительных данных, реализующий интерфейс <see cref="IDataItem"/>.</typeparam>
    public interface IMeasuringSystem<T> : IMeasuringSystem where T : IDataItem
    {
        /// <summary>
        /// Получает последнее успешно зарегистрированное значение (текущие оперативные данные).
        /// </summary>
        T HotValue { get; }

        /// <summary>
        /// Возникает при получении нового пакета данных от измерительной системы.
        /// </summary>
        event EventHandler<DataEventArgs<T>> DataReceived;        
    }
}
