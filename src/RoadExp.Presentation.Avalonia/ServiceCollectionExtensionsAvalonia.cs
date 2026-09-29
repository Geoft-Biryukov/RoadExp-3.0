using Microsoft.Extensions.DependencyInjection;
using RoadExp.Presentation.Avalonia.Models;
using RoadExp.Presentation.Avalonia.ViewModels;

namespace RoadExp.Presentation.Avalonia
{
    internal static class ServiceCollectionExtensionsAvalonia
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainModel>();

            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<SettingsPageViewModel>();
           // services.AddSingleton<ComputerPageViewModel>();
            services.AddSingleton<SettingsPageViewModel>();
          //  services.AddSingleton<ComputerPageViewModel>();

            services.AddSingleton<DistanceSensorPageViewModel>();

            services.AddSingleton<GpsPageViewModel>();
            services.AddSingleton<GeometryPageViewModel>();

            services.AddSingleton<VideoPageViewModel>();
            services.AddSingleton<SkpkPageViewModel>();
        }

        public static void AddMeasuringSystemModels(this IServiceCollection services)
        {
           // services.AddSingleton<IMeasuringSystemModel, ComputerModel>();
           // services.AddSingleton<IMeasuringSystemModel, DistanceSensorModel>();
        }
    }
}
