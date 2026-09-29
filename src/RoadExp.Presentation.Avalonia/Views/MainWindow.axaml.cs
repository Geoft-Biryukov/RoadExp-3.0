using Avalonia.Controls;
using Avalonia.Threading;
using RoadExp.Auxiliaries.Logger;
using System;

namespace RoadExp.Presentation.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RoadExpConsoleSink.OnLogReceived += AddMessageToConsole;
        }

        private void AddMessageToConsole(string message)
        {            
            Dispatcher.UIThread.Post(new Action(() =>
            {
                UserConsole.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
                
                // Автопрокрутка консоли вниз к новому сообщению
                UserConsole.ScrollIntoView(UserConsole.Items[^1]);
            }));
        }

        protected override void OnClosed(EventArgs e)
        {
            RoadExpConsoleSink.OnLogReceived -= AddMessageToConsole;
            base.OnClosed(e);
        }
    }
}