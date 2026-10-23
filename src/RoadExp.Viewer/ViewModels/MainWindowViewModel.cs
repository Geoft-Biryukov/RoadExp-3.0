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
        private const int _rowsOnPage = 20;

        public MainWindowViewModel(MainModel model)
        {
            _model = model;
            
            GpsGgaDataCollection = new ObservableCollection<GpsGgaDataViewModel>();
            GpsRmcDataCollection = new ObservableCollection<GpsRmcDataViewModel>();
            DistanceDataCollection  = new ObservableCollection<DistanceDataViewModel>();
            GeometryDataCollection = new ObservableCollection<GeometryDataViewModel>();

            PropertyChanged += MainWindowViewModelPropertyChanged;
        }

        private void MainWindowViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(nameof(DistanceNumber)))
            {
                SetDistanceData();
            }

            if (e.PropertyName.Equals(nameof(GpsNumber)))
            {
                SetGpsData();
            }

            if (e.PropertyName.Equals(nameof(GeometryNumber)))
            {
                SetGeometryData();
            }
        }

        [ObservableProperty]
        private string _fileName;

        #region gps
        [ObservableProperty]
        private string _gpsFileName;

        [ObservableProperty]
        private ObservableCollection<GpsGgaDataViewModel> _gpsGgaDataCollection;

        [ObservableProperty]
        private ObservableCollection<GpsRmcDataViewModel> _gpsRmcDataCollection;

        [ObservableProperty]
        private int _maximumGpsPageNumber = 1;

        [ObservableProperty]
        private int _gpsNumber = 0;
        #endregion

        #region distance page
        [ObservableProperty]
        private string _distanceFileName;

        [ObservableProperty]
        private ObservableCollection<DistanceDataViewModel> _distanceDataCollection;

        [ObservableProperty]
        private int _maximumDistancePageNumber = 1;

        [ObservableProperty]
        private int _distanceNumber = 0;
        #endregion

        #region geometry
        [ObservableProperty]
        private string _geometryFileName;

        [ObservableProperty]
        private ObservableCollection<GeometryDataViewModel> _geometryDataCollection;

        [ObservableProperty]
        private int _maximumGeometryPageNumber = 1;

        [ObservableProperty]
        private int _geometryNumber = 0;
        #endregion

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

            SetDistanceData();            
        }

        private void SetDistanceData()
        {
            if (_model.DistanceData == null)
                return;

            var data = _model.DistanceData.ProcessedData;

            MaximumDistancePageNumber = data.Count / _rowsOnPage + 1;

            var items = data.Values
                .Skip(_rowsOnPage * DistanceNumber)
                .Take(_rowsOnPage)
                .Select(item => new DistanceDataViewModel(item));

            DistanceDataCollection = new ObservableCollection<DistanceDataViewModel>(items);
        }

        private void UpdateGpsView()
        {
            if (_model.GpsData == null)
                return;

            GpsFileName = _model.GpsData.Path;

            GpsGgaDataCollection.Clear();
            GpsRmcDataCollection.Clear();

            SetGpsData();            
        }

        private void SetGpsData()
        {
            if (_model.GpsData == null)
                return;

            var data = _model.GpsData.ProcessedData;

            MaximumGpsPageNumber = data.Count / _rowsOnPage + 1;

            var ggaItems = data.Values
                 .Select(x => (x.Index, x.Value.Gga))
                 .Skip(_rowsOnPage * GpsNumber)
                 .Take(_rowsOnPage)
                 .Select(item => new GpsGgaDataViewModel(item.Index, item.Gga));

            GpsGgaDataCollection = new ObservableCollection<GpsGgaDataViewModel>(ggaItems);

            var rmcItems = data.Values
                 .Select(x => (x.Index, x.Value.Rmc))
                 .Skip(_rowsOnPage * GpsNumber)
                 .Take(_rowsOnPage)
                 .Select(item => new GpsRmcDataViewModel(item.Index, item.Rmc));

            GpsRmcDataCollection = new ObservableCollection<GpsRmcDataViewModel>(rmcItems);            
        }

        private void UpdateGeometryView()
        {
            if (_model.GeometryData == null)
                return;

            GeometryFileName = _model.GeometryData.Path;

            SetGeometryData();            
        }

        private void SetGeometryData()
        {
            if(_model.GeometryData == null) 
                return;

            var data = _model.GeometryData.ProcessedData;

            MaximumGeometryPageNumber = data.Count / _rowsOnPage + 1;

            var items = data.Values
                 .Select(x => x)
                 .Skip(_rowsOnPage * GeometryNumber)
                 .Take(_rowsOnPage)
                 .Select(item => new GeometryDataViewModel(item));

            GeometryDataCollection = new ObservableCollection<GeometryDataViewModel>(items);
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
