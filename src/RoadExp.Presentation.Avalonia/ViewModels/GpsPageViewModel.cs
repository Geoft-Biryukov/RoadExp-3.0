using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Presentation.Avalonia.Models;
using RoadExp.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class GpsPageViewModel : ViewModelBase
    {
        #region private fields
        private const double _knotsToKmh = 1.852;        
        private readonly IGpsSystemManager _manager;
        private bool _isUpdatingUI = false;

        private readonly Stopwatch _uiUpdateStopwatch = Stopwatch.StartNew();
        // Обновляем интерфейс не чаще, чем раз в 500 миллисекунд (2 Гц)
        private const long _minUpdateIntervalMs = 500;
        #endregion

        #region constructor
        public GpsPageViewModel(MainModel model, IGpsSystemManager manager) : base(model)
        {
            _manager = manager;

            UpdatePortNames();
            UpdateBaudRates();

            PropertyChanged += GpsPageViewModelPropertyChanged;
            _manager.DataReceived += GpsDataReceived;

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
        private string _satellitesCount;

        [ObservableProperty]
        private string _speed;

        [ObservableProperty]
        private string _dateAndTime;

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private string _altitude;

        [ObservableProperty]
        private int _portNameIndex;

        [ObservableProperty]
        private int _baudRateIndex;

        [ObservableProperty]
        private bool _isEmulationMode;

        [ObservableProperty]
        private double _value;
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
        private void GpsDataReceived(object? sender, DataEventArgs<GpsDataItem> e)
        {            
            if (_uiUpdateStopwatch.ElapsedMilliseconds < _minUpdateIntervalMs)
            {
                return;
            }
            
            _uiUpdateStopwatch.Restart();

            var rmc = e.Data.Value.Rmc;
            var gga = e.Data.Value.Gga;

            double speed = rmc.SpeedKnots * _knotsToKmh;            

            Dispatcher.UIThread.Post(() =>
            {
                Latitude = gga.Latitude.ToString("0.000000000");
                Longitude = gga.Longitude.ToString("0.000000000");
                Altitude = gga.Altitude.ToString("0.00");
                SatellitesCount = gga.Satellites.ToString();
                Speed = speed.ToString("0.00");

                DateAndTime = $"{rmc.Date:dd.MM.yyyy}   {rmc.UtcTime:HH:mm:ss}";
                Status = GetStatusString(gga.Quality);

                Value = speed;
            });
        }

        private void GpsPageViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_isUpdatingUI)
                return;

            if (string.Equals(e.PropertyName, nameof(BaudRateIndex)))
            {
                var message = $"Выбрана скорость передачи данных {BaudRates[BaudRateIndex]}";               
            }

            if (string.Equals(e.PropertyName, nameof(PortNameIndex)))
            {
                var message = $"Выбран COM-порт {PortNames[PortNameIndex]}";               
            }
            
            if (string.Equals(e.PropertyName, nameof(IsEmulationMode)))
            {
                var message = IsEmulationMode ? "Эмуляция включена" : "Эмуляция выключена";              
            }

            UpdateModel();
        }
        #endregion

        #region static helpers
        private static string GetStatusString(int status)
        {
            var result = string.Empty;

            switch (status)
            {
                case 0: result = "Fix not valid"; break;
                case 1: result = "GPS fix"; break;
                case 2: result = "Differential GPS fix (DGNSS), SBAS, OmniSTAR VBS, Beacon, RTX in GVBS mode"; break;
                case 3: result = "Not applicable"; break;
                case 4: result = "RTK Fixed, xFill"; break;
                case 5: result = " RTK Float, OmniSTAR XP/HP, Location RTK, RTX"; break;
                case 6: result = "INS Dead reckoning"; break;
                default:
                    result = string.Empty;                    
                    break;
            }

            return result;
        }
        #endregion
    }
}
