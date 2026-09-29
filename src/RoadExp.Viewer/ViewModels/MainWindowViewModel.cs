using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RoadExp.Viewer.Models;
using RoadExp.Viewer.Reader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Viewer.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly MainModel _model;
        
        public MainWindowViewModel(MainModel model)
        {
            _model = model;
            
            GpsGgaDataCollection = new ObservableCollection<GpsGgaDataViewModel>();
            GpsRmcDataCollection = new ObservableCollection<GpsRmcDataViewModel>();
            DistanceDataCollection  = new ObservableCollection<DistanceDataViewModel>();
            GeometryDataCollection = new ObservableCollection<GeometryDataViewModel>();
        }

        [ObservableProperty]
        private string _fileName;

        [ObservableProperty]
        private string _gpsFileName;

        [ObservableProperty]
        private ObservableCollection<GpsGgaDataViewModel> _gpsGgaDataCollection;

        [ObservableProperty]
        private ObservableCollection<GpsRmcDataViewModel> _gpsRmcDataCollection;

        [ObservableProperty]
        private string _distanceFileName;

        [ObservableProperty]
        private ObservableCollection<DistanceDataViewModel> _distanceDataCollection;

        [ObservableProperty]
        private string _geometryFileName;

        [ObservableProperty]
        private ObservableCollection<GeometryDataViewModel> _geometryDataCollection;

        public async Task OpenFileCommand()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Открытие файла данных Дорога ПРО",
                    AllowMultiple = false,
                    FileTypeFilter = new List<FilePickerFileType>
                    {
                        new FilePickerFileType("Файл данных Дорога ПРО") { Patterns = ["*.road"] },
                        new FilePickerFileType("Все файлы") { Patterns = ["*.*"] }
                    }
                });

                if (files.Count == 0)
                    return;                

                var path = files.First().TryGetLocalPath();
                var dataReader = new DataReader();

                var result = await dataReader.ReadDataAsync(path);               

                if (result.IsSuccess)
                {
                    var fileData = result.Value;

                    var header = fileData.FileHeader;
                    var metadata = fileData.Metadata;
                    var data = fileData.Batches;
                    
                    var messageBox = MessageBoxManager.GetMessageBoxStandard(
                        "Загрузка файла", 
                        $"Файл успешно загружен{Environment.NewLine}" +
                        $"{Environment.NewLine}" +
                        $"код: {Encoding.ASCII.GetString(header.UniqueCode)}{Environment.NewLine}" +
                        $"система: {header.SystemId}{Environment.NewLine}" +
                        $"время: {new DateTime(header.TimeUtc)}{Environment.NewLine}" +
                        $"кол-во данных: {data.Count}", 
                        ButtonEnum.Ok);

                    await messageBox.ShowAsync();

                    FileName = path;

                    _model.SetFileData(path, fileData);

                    UpdateView();
                }
                else
                {
                    var messageBox = MessageBoxManager.GetMessageBoxStandard(
                        "Загрузка файла",
                        $"Ошибка при чтении файла{Environment.NewLine}" +
                        $"{result.Errors.Select(e => e.Message).First()}",
                        ButtonEnum.Ok);

                    await messageBox.ShowAsync();
                }                               
            }
        }

        private void UpdateView()
        {
            UpdateDistanceView();
            UpdateGpsView();
            UpdateGeometryView();
        }

       
        private void UpdateDistanceView()
        {
            if(_model.DistanceData == null) 
                return;

            DistanceFileName = _model.DistanceData.Path;

            DistanceDataCollection.Clear();

            var data = _model.DistanceData.ProcessedData;

            foreach (var entry in data)
            {
                var item = new DistanceDataViewModel(entry.Value);
                DistanceDataCollection.Add(item);
            }
        }

        private void UpdateGpsView()
        {
            if (_model.GpsData == null)
                return;

            GpsFileName = _model.GpsData.Path;

            GpsGgaDataCollection.Clear();
            GpsRmcDataCollection.Clear();

            var data = _model.GpsData.ProcessedData;

            foreach(var entry in data)
            {
                var itemGga = new GpsGgaDataViewModel(entry.Value.Index, entry.Value.Value.Gga);
                GpsGgaDataCollection.Add(itemGga);

                var itemRmc = new GpsRmcDataViewModel(entry.Value.Index, entry.Value.Value.Rmc);
                GpsRmcDataCollection.Add(itemRmc);
            }
        }

        private void UpdateGeometryView()
        {
            if (_model.GeometryData == null)
                return;

            GeometryFileName = _model.GeometryData.Path;

            GeometryDataCollection.Clear();

            var data = _model.GeometryData.ProcessedData;

            foreach (var entry in data)
            {
                var item = new GeometryDataViewModel(entry.Value);
                GeometryDataCollection.Add(item);
            }
        }


        public void ExitCommand()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}
