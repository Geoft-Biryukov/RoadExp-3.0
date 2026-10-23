using CommunityToolkit.Mvvm.ComponentModel;
using RoadExp.DistanceMeasuringSystem.Managers;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.DistanceMeasuringSystem;
using RoadExp.Presentation.Avalonia.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class DistanceSensorPageViewModel : ViewModelBase
    {        
        private readonly IDistanceSystemManager _manager;
        private bool _isUpdatingUI;
        public DistanceSensorPageViewModel(MainModel model, IDistanceSystemManager manager) : base(model)
        {            
            _manager = manager;

            PropertyChanged += OnPropertyChanged;
            _manager.DataReceived += SystemDataReceived;

            UpdateUI();
        }

        private void SystemDataReceived(object? sender, DataEventArgs<DistanceDataItem> e)
        {
            ImpulseCounter = e.Data.Value.ImpulseCount.ToString();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_isUpdatingUI)
                return;

            if (e.PropertyName.Equals(nameof(IsEmulationMode)))
            {
                _manager.Settings.IsEmulationMode = IsEmulationMode;
            }

            UpdateModel();
        }

        public ObservableCollection<string> Polarities { get; } = new ObservableCollection<string>
        {
            "Положительная", 
            "Отрицательная"
        };

        public ObservableCollection<string> Reverse { get; } = new ObservableCollection<string>()
        {
            "Двухфазный(реверсивный)",
            "Однофазный(нереверсивный)"
        };

        [ObservableProperty]
        private string _impulseCounter;

        [ObservableProperty]
        private string _distance;

        [ObservableProperty]
        private string _speed;

        [ObservableProperty]
        private double _correctionFactor;

        [ObservableProperty]
        private int _polarityIndex;

        [ObservableProperty]
        private int _reverseIndex;

        [ObservableProperty]
        private bool _isEmulationMode;
            
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

        public void UpdateUI()
        {
            _isUpdatingUI = true;
            
            IsEmulationMode = _manager.IsEmulationMode;

            _isUpdatingUI = false;
        }

        public void UpdateModel()
        {
            if (_isUpdatingUI)
                return;            

            _manager.IsEmulationMode = IsEmulationMode;
        }

    }
}
