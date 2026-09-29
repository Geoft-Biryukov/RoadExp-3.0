using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Presentation.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ListItemTemplate> templateList = 
            [
                new ListItemTemplate(typeof(HomePageViewModel), "Все системы", "HomeRegular"),
                new ListItemTemplate(typeof(SettingsPageViewModel), "Настройки", "SettingsRegular"),
                //new ListItemTemplate(typeof(ComputerPageViewModel), "Тестовая система", "TestRegular"),
                new ListItemTemplate(typeof(DistanceSensorPageViewModel), "ДПП", "DistanceRegular"),
                new ListItemTemplate(typeof(GpsPageViewModel), "GPS-система", "GpsRegular"),
                new ListItemTemplate(typeof(GeometryPageViewModel), "Геометрия", "GeometryRegular"),
                new ListItemTemplate(typeof(VideoPageViewModel), "Видео", "VideoRegular"),
                new ListItemTemplate(typeof(SkpkPageViewModel), "СКПК", "SkpkRegular"),
            ];
        private readonly ServiceProvider _provider;
        private readonly ILogger<MainWindowViewModel> _logger;

        private readonly Dictionary<Type, ViewModelBase> _viewModels = new Dictionary<Type, ViewModelBase>();

        public MainWindowViewModel(MainModel model, ServiceProvider provider, ILogger<MainWindowViewModel> logger) : base(model)
        {            
            _provider = provider;
            _logger = logger;

            if (model.SettingsModel.LoadSettings())
            {
                _logger.UserInformation("Настройки загружены успешно");
            }
            else
            {
                _logger.UserWarning("Не удалось загрузить настройки. Используются настройки по-умолчанию");
            }

            _logger.LogDebug("Запуск главного окна");

            PropertyChanged += MainWindowViewModelPropertyChanged;

            foreach (var template in templateList)
            {
                var instance = _provider.GetRequiredService(template.ModelType);
                _viewModels.Add(template.ModelType, (ViewModelBase)instance);
            }

            // IsPaneOpen = true;

            //UserLog.Information("Приложение запущено");                         
        }

       
        public ObservableCollection<ListItemTemplate> Items
        {
            get => templateList;
        }        

        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private ListItemTemplate _selectedItem;

        [ObservableProperty]
        private ViewModelBase _currentPage;
        
        public void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private void SelectView(ListItemTemplate? value)
        {
            if (value is null) return;                        

            //var instance = _provider.GetRequiredService(value.ModelType);
            
            //if (instance is null) return;
            CurrentPage = _viewModels[value.ModelType];// (ViewModelBase)instance;
        }

        private void MainWindowViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(SelectedItem)))
            {
                SelectView(SelectedItem);
                _logger.LogInformation($"Выбрана система: {SelectedItem.Label}");
            }
        }       
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string label, string iconKey)
        {
            ModelType = type;
            Label = label;

            Application.Current!.TryFindResource(iconKey, out var resource);

            ListItemIcon = (StreamGeometry)resource!;
        }

        public string Label { get; }
        public Type ModelType { get; }
        public StreamGeometry ListItemIcon { get; }
    }      
}
