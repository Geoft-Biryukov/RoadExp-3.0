using RoadExp.Measurements.Core;

namespace RoadExp.Infrastructure
{
    public class MeasuringSystemsManager
    {
        private readonly Dictionary<string, IMeasuringSystemsFactory> _factories = new Dictionary<string, IMeasuringSystemsFactory>();

        public void RegisterSystemFactory<TSettings, TData>(string systemName, IMeasuringSystemsFactory<TSettings, TData> factory)
            where TSettings : MeasuringSystemSettings
            where TData : IDataItem
        {
            _factories.Add(systemName, factory);
        }

        public IMeasuringSystem<TData> CreateSystem<TData, TSettings>(string systemName, MeasuringSystemSettings settings)
            where TSettings : MeasuringSystemSettings
            where TData : IDataItem
        {
            if (!_factories.TryGetValue(systemName, out var suitableFactory))
                throw new ArgumentException($"Измерительная система {systemName} не зарегистрирована");

            if (settings is not TSettings)
                throw new ArgumentException($"{nameof(settings)} неправильный тип настроек измерительной системы");

            var suitableSettings = (TSettings)settings;

            var factory = (IMeasuringSystemsFactory<TSettings, TData>)suitableFactory;

            return factory.CreateMeasuringSystem(suitableSettings);
        }            
    }
}
