namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет базовый маркерный контракт для фабрик измерительных систем.
    /// </summary>
    public interface IMeasuringSystemsFactory
    {

    }

    /// <summary>
    /// Определяет контракт фабрики для создания и конфигурации строго типизированных измерительных систем на основе настроек.
    /// </summary>
    /// <typeparam name="TSettings">Тип настроек измерительной системы, наследуемый от <see cref="MeasuringSystemSettings"/>.</typeparam>
    /// <typeparam name="TData">Тип регистрируемых измерительных данных, реализующий интерфейс <see cref="IDataItem"/>.</typeparam>
    public interface IMeasuringSystemsFactory<in TSettings, TData> : IMeasuringSystemsFactory
        where TSettings : MeasuringSystemSettings
        where TData : IDataItem
    {
        /// <summary>
        /// Создает, конфигурирует и возвращает новый экземпляр измерительной системы с использованием указанных настроек.
        /// </summary>
        /// <param name="settings">Конфигурационные настройки, необходимые для инициализации измерительной системы.</param>
        /// <returns>Новый экземпляр измерительной системы, реализующий <see cref="IMeasuringSystem{TData}"/>.</returns>
        IMeasuringSystem<TData> CreateMeasuringSystem(TSettings settings);       
    }
}
