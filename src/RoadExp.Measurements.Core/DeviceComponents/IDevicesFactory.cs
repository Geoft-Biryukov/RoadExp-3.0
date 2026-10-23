namespace RoadExp.Measurements.Core.DeviceComponents
{
    /// <summary>
    /// Определяет контракт фабрики для создания device'ов 
    /// на основе конфигурационных настроек.
    /// </summary>
    /// <typeparam name="TSettings">Тип настроек измерительной системы, наследуемый от <see cref="MeasuringSystemSettings"/>.</typeparam>
    public interface IDevicesFactory<TSettings> : IMeasurementErrorProvider
        where TSettings : MeasuringSystemSettings
    {
        /// <summary>
        /// Создает и конфигурирует новый список Device'ов с использованием указанных настроек.
        /// </summary>
        /// <param name="settings">Конфигурационные настройки, необходимые для создания Device'ов.</param>
        /// <returns>Список Device'ов измерительной системы.</returns>
        IReadOnlyList<IDevice> CreateDevices(TSettings settings);
    }
}
