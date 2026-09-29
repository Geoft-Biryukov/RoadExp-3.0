using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RoadExp.Viewer.Models;
using RoadExp.Viewer.ViewModels;
using RoadExp.Viewer.Views;

namespace RoadExp.Viewer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                var services = new ServiceCollection();

                services.AddDataParsers();
                services.AddSingleton<MainModel>();

                var provider = services.BuildServiceProvider();

                var model = provider.GetRequiredService<MainModel>();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(model),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            //var dataValidationPluginsToRemove =
            //    BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            //// remove each entry found
            //foreach (var plugin in dataValidationPluginsToRemove)
            //{
            //    BindingPlugins.DataValidators.Remove(plugin);
            //}
        }
    }
}