using CommunityToolkit.Mvvm.ComponentModel;
using RoadExp.Presentation.Avalonia.Models;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected ViewModelBase(MainModel model)
        {
            Model = model;
        }

        public MainModel Model { get; set; }
    }
}
