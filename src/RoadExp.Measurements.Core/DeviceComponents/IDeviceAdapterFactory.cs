namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет контракт фабрики для создания адаптеров физических устройств 
    /// на основе конфигурационных настроек.
    /// </summary>
    /// <typeparam name="TSettings">Тип настроек измерительной системы, наследуемый от <see cref="MeasuringSystemSettings"/>.</typeparam>
    public interface IDeviceAdapterFactory<in TSettings>
        where TSettings : MeasuringSystemSettings
    {
        /// <summary>
        /// Создает и конфигурирует новый экземпляр адаптера устройства с использованием указанных настроек.
        /// </summary>
        /// <param name="settings">Конфигурационные настройки, необходимые для инициализации адаптера.</param>
        /// <returns>Новый экземпляр адаптера устройства, реализующий <see cref="IDeviceAdapter"/>.</returns>
        IDeviceAdapter Create(TSettings settings);

    }
}
