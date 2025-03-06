using Microsoft.Extensions.DependencyInjection;
using SVModManager.ViewModel;
using SVModManager.View;
using System.Configuration;
using System.Data;
using System.Transactions;
using System.Windows;

namespace SVModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;
        public ServiceProvider ServiceProvider { get; set; }

        public App()
        {
            var container = new ServiceCollection();

            container.AddSingleton<MainWindow>();
            container.AddSingleton<NavigationVM>();

            container.AddSingleton<ModListVM>();
            container.AddSingleton<DownLoadVM>();
            container.AddSingleton<SettingsVM>();

            ServiceProvider = container.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }

}


