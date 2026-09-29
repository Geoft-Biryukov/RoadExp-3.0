using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Presentation.Avalonia.Models;
using RoadExp.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class GeometryPageViewModel : ViewModelBase
    {
        #region private fields        
        private readonly IGeometrySystemManager _manager;
        private bool _isUpdatingUI = false;
        #endregion

        #region constructor
        public GeometryPageViewModel(MainModel model, IGeometrySystemManager manager) : base(model)
        {
            _manager = manager;

            UpdatePortNames();
            UpdateBaudRates();

            PropertyChanged += GeometryPageViewModelPropertyChanged;
            _manager.DataReceived += SystemDataReceived;

            UpdateUI();           
        }

        #endregion

        #region properties

        public ObservableCollection<string> PortNames { get; } = new ObservableCollection<string>();

        public ObservableCollection<int> BaudRates { get; } = new ObservableCollection<int>();

        [ObservableProperty]
        private string _latitude;

        [ObservableProperty]
        private string _longitude;
        
        [ObservableProperty]
        private string _altitude;

        [ObservableProperty]
        private string _longitudinalGradient;

        [ObservableProperty]
        private string _transverseSlope;

        [ObservableProperty]
        private string _course;

        [ObservableProperty]
        private int _portNameIndex;

        [ObservableProperty]
        private int _baudRateIndex;

        [ObservableProperty]
        private bool _isEmulationMode;
        #endregion

        #region public methods
        public void UpdateModel()
        {
            if (_isUpdatingUI)
                return;

            if (PortNameIndex >= 0)
            {
                _manager.PortName = PortNames[PortNameIndex];
            }

            if (BaudRateIndex >= 0)
            {
                _manager.BaudRate = BaudRates[BaudRateIndex];
            }

            _manager.IsEmulationMode = IsEmulationMode;
        }

        public async Task Update()
        {
            _manager.UpdateParameters();
        }

        public void UpdateUI()
        {
            _isUpdatingUI = true;

            PortNameIndex = PortNames.IndexOf(_manager.PortName);
            BaudRateIndex = BaudRates.IndexOf(_manager.BaudRate);
            IsEmulationMode = _manager.IsEmulationMode;

            _isUpdatingUI = false;
        }

        public async Task Initialize()
        {
            await _manager.InitializeAsync();
        }

        public void Deinitialize()
        {
            _manager.Deinitialize();
        }

        public async Task Start()
        {
            var filePath = Model.SettingsModel.Settings.FolderToSaveData;
            _manager.StartAcquisition(filePath);
        }

        public async Task Stop()
        {
            _manager.StopAcquisition();
        }
        #endregion

        #region private methods
        private void UpdateBaudRates()
        {
            var baudRates = SerialPortHelpers.BaudRates;
            foreach (var baudRate in baudRates)
            {
                BaudRates.Add(baudRate);
            }
        }

        private void UpdatePortNames()
        {
            var portNames = SerialPortHelpers.GetNames();

            foreach (var name in portNames)
            {
                PortNames.Add(name);
            }
        }
        #endregion

        #region event handlers
        private void SystemDataReceived(object? sender, DataEventArgs<GeometryDataItem> e)
        {
            var value = e.Data.Value;

            Dispatcher.UIThread.Post(() =>
            {
                Longitude = value.Longitude.ToString("N5");
                Latitude = value.Latitude.ToString("N5");
                Altitude = value.HeightGPS.ToString("N3");

                TransverseSlope = value.CrossHeel.ToString("N5");
                LongitudinalGradient = value.LongHeel.ToString("N5");

                Course = value.Course.ToString("N5");
            });
        }

        private void GeometryPageViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateModel();
        }
        #endregion
    }
}
