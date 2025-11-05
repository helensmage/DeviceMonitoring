using DeviceMonitoring.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using DeviceMonitoring.ViewModels;

namespace DeviceMonitoring
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainViewModel = new MainViewModel();
            MainWindow = new MainWindow { DataContext = mainViewModel };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
