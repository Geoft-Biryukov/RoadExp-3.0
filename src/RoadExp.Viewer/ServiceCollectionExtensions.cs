using Microsoft.Extensions.DependencyInjection;
using RoadExp.DistanceMeasuringSystem.DataConverters;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.Viewer
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataParsers(this IServiceCollection services)
        {
            services.AddSingleton<IByteArrayConverter<DistanceRawData>, DistanceDataConverter>();
            services.AddSingleton<IByteArrayParser<DistanceDataItem>, DistanceDataParser>();

            services.AddSingleton<IByteArrayParser<GpsDataItem>, GpsDataParser>();

            services.AddSingleton<IByteArrayConverter<GeometryRawData>, GeometryDataConverter>();
            services.AddSingleton<IByteArrayParser<GeometryDataItem>, GeometryDataParser>();
        }
    }
}
