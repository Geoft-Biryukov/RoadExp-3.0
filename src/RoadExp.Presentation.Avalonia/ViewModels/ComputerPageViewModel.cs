using CommunityToolkit.Mvvm.ComponentModel;
using RoadExp.Presentation.Avalonia.Models;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class ComputerPageViewModel : ViewModelBase
    {
        private readonly ComputerModel _model;

        public ComputerPageViewModel(MainModel model) : base(model)
        {
            _model = model.GetModel(SystemsList.TestSystemId) as ComputerModel;

            _model.DataReady += ModelDataReady;
        }

        [ObservableProperty]
        private string _processorUsage;

        [ObservableProperty]
        private string _availableMemory;

        public void CreateSystem()
        {
            _model.CreateSystem();
        }

        public async Task InitializeSystem()
        {
            await _model.InitializeSystemAsync();
        }

        public void StartAcquisition()
        {
            _model.StartAcquisition();
        }

        public void StopAcquisition()
        {
            _model.StopAcquisition();
        }

        private void ModelDataReady(object? sender, System.EventArgs e)
        {
            ProcessorUsage = _model.Data.ProcessorUsage.ToString();
            AvailableMemory = _model.Data.AvailableMemory.ToString();
        }
    }
}
