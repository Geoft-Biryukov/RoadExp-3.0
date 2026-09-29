using Microsoft.Extensions.DependencyInjection;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.DistanceMeasuringSystem.DeviceComponents;
using RoadExp.DistanceMeasuringSystem.Managers;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GeometryMeasuringSystem.Managers;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Measurements.Common.IndexProviders;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMeasuringSystemManagers(this IServiceCollection services)
        {
            services.AddSingleton<IIndexProvider, TimestampIndexProvider>();

            AddDistance(services);
            AddGps(services);
            AddGeometry(services);
        }

        private static void AddDistance(IServiceCollection services)
        {
            services.AddSingleton<IByteArrayParser<DistanceDataItem>, DistanceDataParser>();
            services.AddSingleton<IByteArrayConverter<DistanceRawData>, DistanceDataConverter>();
            services.AddSingleton<IDeviceAdapterFactory<DistanceSystemSettings>, DistanceDeviceAdapterFactory>();
            services.AddSingleton<IMeasuringSystemSettingsManager<DistanceSystemSettings>, DistanceSystemSettingsManager>();
            services.AddSingleton<IMeasuringSystemsFactory<DistanceSystemSettings, DistanceDataItem>, DistanceSystemFactory>();
            services.AddSingleton<IDistanceSystemManager, DistanceSystemManager>();

            services.AddSingleton<IMeasuringSystemManager>(x => x.GetRequiredService<IDistanceSystemManager>());
        }

        private static void AddGps(IServiceCollection services)
        {
            services.AddSingleton<IByteArrayParser<GpsDataItem>, GpsDataParser>();
            services.AddSingleton<IByteArrayConverter<GpsRawData>, GpsDataConverter>();
            services.AddSingleton<IDeviceAdapterFactory<GpsSystemSettings>, GpsDeviceAdapterFactory>();
            services.AddSingleton<IMeasuringSystemSettingsManager<GpsSystemSettings>, GpsSystemSettingsManager>();
            services.AddSingleton<IMeasuringSystemsFactory<GpsSystemSettings, GpsDataItem>, GpsSystemFactory>();
            services.AddSingleton<IGpsSystemManager, GpsSystemManager>();

            services.AddSingleton<IMeasuringSystemManager>(x => x.GetRequiredService<IGpsSystemManager>());
        }

        private static void AddGeometry(IServiceCollection services)
        {
            services.AddSingleton<IByteArrayParser<GeometryDataItem>, GeometryDataParser>();
            services.AddSingleton<IByteArrayConverter<GeometryRawData>, GeometryDataConverter>();
            services.AddSingleton<IDeviceAdapterFactory<GeometrySystemSettings>, GeometryDeviceAdapterFactory>();
            services.AddSingleton<IMeasuringSystemSettingsManager<GeometrySystemSettings>, GeometrySystemSettingsManager>();
            services.AddSingleton<IMeasuringSystemsFactory<GeometrySystemSettings, GeometryDataItem>, GeometrySystemFactory>();
            services.AddSingleton<IGeometrySystemManager, GeometrySystemManager>();

            services.AddSingleton<IMeasuringSystemManager>(x => x.GetRequiredService<IGeometrySystemManager>());
        }
    }
}
