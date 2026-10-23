using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Infrastructure;
using RoadExp.Measurements.Common;
using RoadExp.Presentation.Avalonia.Models;
using RoadExp.Presentation.Avalonia.ViewModels;
using RoadExp.Presentation.Avalonia.Views;
using Serilog;
using Serilog.Filters;
using System.Linq;

namespace RoadExp.Presentation.Avalonia
{
    public partial class App : Application
    {
        private MainModel _mainModel;
        private ILoggerFactory _loggerFactory;
        

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);           
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                AdjustLogger();
                
                var logger = _loggerFactory.CreateLogger<MainWindowViewModel>();
                logger.LogInformation("---Приложение запущено---");

                var services = new ServiceCollection();

                services.AddLogging();
                services.AddSerilog();              

                services.AddMeasuringSystemManagers(_loggerFactory);
                services.AddMeasuringSystemModels();
               
                services.AddViewModels();              

                var provider = services.BuildServiceProvider();

                if (desktop is IControlledApplicationLifetime controlled)
                {                    
                    controlled.Exit += OnExit;
                }

                

                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                _mainModel = provider.GetRequiredService<MainModel>();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(_mainModel, provider, logger),
                };

                
            }

            base.OnFrameworkInitializationCompleted();
        }       

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
        
        private void AdjustLogger()
        {            
            // Настраиваем Serilog: для каждой системы имеется возможность 
            // писать логи в свой файл
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()            
            .WriteTo.File("logs/app-debug.txt",
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                                rollingInterval: RollingInterval.Day)            
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToUser"))
                .WriteTo.RoadExpConsole())
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToGpsSystem"))
                .WriteTo.File("logs/gps-system.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToGeometrySystem"))
                .WriteTo.File("logs/geometry-system.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToTestSystem"))
                .WriteTo.File("logs/test-system.txt", rollingInterval: RollingInterval.Day))
            .CreateLogger();

            //Создаем стандартную фабрику логов и связываем её с Serilog
           _loggerFactory = LoggerFactory.Create(builder =>
           {
               // Метод расширения AddSerilog связывает Microsoft-логирование с Serilog
               builder.AddSerilog(dispose: true);
           });
        }

        private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            Log.CloseAndFlush();
            _mainModel.Dispose();
            _mainModel.SettingsModel.SaveSettings();
        }
    }
}