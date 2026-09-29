using System.Threading.Channels;

namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет низкоуровневый контракт адаптера для взаимодействия с физическим или виртуальным измерительным устройством.
    /// </summary>    
    public interface IDeviceAdapter : IMeasurementErrorProvider, IDisposable
    {
        /// <summary>
        /// Канал для чтения входящих пакетов данных без аллокаций.
        /// </summary>
        ChannelReader<MeasurementBatch> DataChannel { get; }

        /// <summary>
        /// Асинхронно инициализирует адаптер и устанавливает соединение с физическим устройством.
        /// </summary>
        /// <param name="token">Токен отмены для прерывания операции инициализации и подключения.</param>
        /// <returns>Задача, представляющая асинхронную операцию инициализации.</returns>
        Task InitializeAsync(CancellationToken token);

        /// <summary>
        /// Деинициализирует адаптер, корректно закрывает соединение с устройством и переводит его в исходное состояние.
        /// </summary>
        void Deinitialize();

    }
}
