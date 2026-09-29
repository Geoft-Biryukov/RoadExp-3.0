using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoadExp.Presentation.Avalonia.Models;
using System.Threading.Tasks;

namespace RoadExp.Presentation.Avalonia.ViewModels
{
    public partial class SettingsPageViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;

        public SettingsPageViewModel(MainModel model) : base(model)
        {
            _settingsModel = model.SettingsModel;
            UpdateSettings();
        }

        public void UpdateSettings()
        {
            CurrentPath = _settingsModel.Settings.FolderToSaveData;
        }


        [ObservableProperty]
        private string _currentPath;

        [RelayCommand]
        public async Task ChooseDirectoryAsync()
        {
            // 1. Safely retrieve the current window lifetime
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                && desktop.MainWindow is not null)
            {
                // 2. Get the TopLevel visually containing the main window
                var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);

                if (topLevel != null)
                {
                    // 3. Prompt directory selection
                    var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        Title = "Выбор пути",
                        AllowMultiple = false
                    });

                    if (folders.Count > 0)
                    {
                        string selectedPath = folders[0].Path.LocalPath;
                        
                        CurrentPath = selectedPath;
                        _settingsModel.Settings.FolderToSaveData = selectedPath;
                    }
                }
            }
        }
    }
}
