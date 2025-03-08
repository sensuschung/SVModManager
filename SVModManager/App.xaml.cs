using Microsoft.Extensions.DependencyInjection;
using SVModManager.ViewModel;
using SVModManager.Services;
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

            container.AddSingleton<DbService>();
            container.AddSingleton<FileHandler>();
            container.AddSingleton<JsonService>();
            container.AddSingleton<ModService>();

            ServiceProvider = container.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }

}


