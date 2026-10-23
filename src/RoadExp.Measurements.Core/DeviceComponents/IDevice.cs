using RoadExp.Measurements.Core.Serialization;
using System.Threading.Channels;

namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет высокоуровневый контракт для управления аппаратным устройством, сбора и сохранения данных.
    /// Zero allocation
    /// </summary>
    public interface IDevice : IMeasurementErrorProvider, IAsyncDisposable
    {
        /// <summary>
        /// Канал для чтения входящих пакетов данных.
        /// </summary>
        ChannelReader<MeasurementBatch> DataChannel { get; }

        /// <summary>
        /// Инициализирует аппаратные и программные ресурсы устройства для подготовки к работе.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Прерывает текущий выполняющийся процесс инициализации устройства.
        /// </summary>
        void CancelInitialization();

        /// <summary>
        /// Освобождает инициализированные ресурсы устройства и переводит его в исходное состояние.
        /// </summary>
        void Deinitialize();        
      
        /// <summary>
        /// Запускает процесс непрерывного сбора данных с устройства и их параллельной записи в файл.
        /// </summary>
        /// <param name="dataFileParams">Параметры файла данных</param>
        void StartAcquisition(DataFileParameters dataFileParams);

        /// <summary>
        /// Останавливает процесс сбора данных и прекращает их запись в файл.
        /// </summary>
        void StopAcquisition();        
    }
}
