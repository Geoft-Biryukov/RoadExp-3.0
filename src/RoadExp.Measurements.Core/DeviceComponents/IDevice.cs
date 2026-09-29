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
        /// Создает или открывает целевой файл, в который будет осуществляться потоковая запись собираемых данных.
        /// </summary>
        /// <param name="fileName">Путь к файлу или имя создаваемого файла.</param>
        /// <param name="fileHeader">Общий заголовок файла с метаданными измерительной сессии.</param>
        /// <param name="metadata">Дополнительный массив байт с пользовательскими или системными метаданными.</param>
        void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata);

        /// <summary>
        /// Запускает процесс непрерывного сбора данных с устройства и их параллельной записи в открытый файл.
        /// </summary>
        void StartAcquisition();

        /// <summary>
        /// Останавливает процесс сбора данных и прекращает их запись в файл.
        /// </summary>
        void StopAcquisition();

        /// <summary>
        /// Безопасно заменяет текущий низкоуровневый адаптер связи с физическим устройством на новый, 
        /// предварительно останавливая сбор данных и деинициализируя старый адаптер.
        /// </summary>
        /// <param name="adapter">Новый экземпляр адаптера устройства <see cref="IDeviceAdapter"/>.</param>        
        void UpdateAdapter(IDeviceAdapter adapter);
    }
}
