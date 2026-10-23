using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.DistanceMeasuringSystem.DeviceComponents;
using RoadExp.DistanceMeasuringSystem.Managers;
using RoadExp.DistanceMeasuringSystem.MeasuringSystem;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GeometryMeasuringSystem.DeviceComponents;
using RoadExp.GeometryMeasuringSystem.Managers;
using RoadExp.GpsMeasuringSystem;
using RoadExp.GpsMeasuringSystem.DeviceComponents;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Common.IndexProviders;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.DeviceComponents;
using RoadExp.Measurements.Core.Serialization;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Measurements.Data.VideoRecordingSystem;
using RoadExp.VideoRecordingSystem.DataConverters;
using RoadExp.VideoRecordingSystem.DeviceComponents;
using RoadExp.VideoRecordingSystem.Managers;
using RoadExp.VideoRecordingSystem.MeasuringSystem;

namespace RoadExp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMeasuringSystemManagers(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            services.AddTransient(services => loggerFactory.CreateLogger<DataWriter>());
            services.AddTransient(services => loggerFactory.CreateLogger<DataStorage>());

            services.AddSingleton<ITriggerProvider, TimeTriggerProvider>();
            services.AddSingleton<IIndexProvider, TimestampIndexProvider>();
            services.AddTransient<IFileHeaderFactory, FileHeaderFactory>();
            services.AddTransient<IDataWriter, DataWriter>();
            services.AddTransient<IDataStorage, DataStorage>();

            AddDistance(services);
            AddGps(services);
            AddGeometry(services);
            AddVideoRecording(services);
        }

        private static void AddDistance(IServiceCollection services)
        {
            services.AddSingleton<IByteArrayParser<DistanceDataItem>, DistanceDataParser>();
            services.AddSingleton<IByteArrayConverter<DistanceRawData>, DistanceDataConverter>();
            services.AddSingleton<IDevicesFactory<DistanceSystemSettings>, DistanceDevicesFactory>();
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
            services.AddSingleton<IDevicesFactory<GpsSystemSettings>, GpsDevicesFactory>();
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
            services.AddSingleton<IDevicesFactory<GeometrySystemSettings>, GeometryDevicesFactory>();
            services.AddSingleton<IDeviceAdapterFactory<GeometrySystemSettings>, GeometryDeviceAdapterFactory>();
            services.AddSingleton<IMeasuringSystemSettingsManager<GeometrySystemSettings>, GeometrySystemSettingsManager>();
            services.AddSingleton<IMeasuringSystemsFactory<GeometrySystemSettings, GeometryDataItem>, GeometrySystemFactory>();
            services.AddSingleton<IGeometrySystemManager, GeometrySystemManager>();

            services.AddSingleton<IMeasuringSystemManager>(x => x.GetRequiredService<IGeometrySystemManager>());
        }

        private static void AddVideoRecording(IServiceCollection services)
        {
            services.AddSingleton<IByteArrayParser<VideoDataItem>, VideoDataParser>();
            //services.AddSingleton<IByteArrayConverter<GeometryRawData>, GeometryDataConverter>();
            services.AddSingleton<IDevicesFactory<VideoSystemSettings>, VideoDevicesFactory>();
          //  services.AddSingleton<IDeviceAdapterFactory<GeometrySystemSettings>, GeometryDeviceAdapterFactory>();
            services.AddSingleton<IMeasuringSystemSettingsManager<VideoSystemSettings>, VideoSystemSettingsManager>();
            services.AddSingleton<IMeasuringSystemsFactory<VideoSystemSettings, VideoDataItem>, VideoSystemFactory>();
            services.AddSingleton<IVideoSystemManager, VideoSystemManager>();

            services.AddSingleton<IMeasuringSystemManager>(x => x.GetRequiredService<IVideoSystemManager>());
        }
    }
}
