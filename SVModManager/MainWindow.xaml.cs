using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SVModManager.ViewModel;

namespace SVModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly NavigationVM _model;
        public MainWindow(NavigationVM model)
        {
            InitializeComponent();
            DataContext = model;
            _model = model;
        }

        private void NaviBtn_Click(object sender, RoutedEventArgs e)
        {
            var s = (RadioButton)e.OriginalSource;
            switch (s.Name)
            {
                case "list_btn":
                    _model.NavigateTo<ModListVM>();
                    break;
                case "download_btn":
                    _model.NavigateTo<DownLoadVM>();
                    break;
                case "settings_btn":
                    _model.NavigateTo<SettingsVM>();
                    break;
                default:
                    _model.NavigateTo<ModListVM>();
                    break;

            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void list_btn_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}