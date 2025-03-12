using Microsoft.Extensions.DependencyInjection;
using SVModManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SVModManager.View
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = App.Current.ServiceProvider.GetRequiredService<SettingsVM>();
        }

        private void APITextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                APITextBlock.Visibility = Visibility.Collapsed;
                APITextBox.Visibility = Visibility.Visible;     
                APITextBox.Focus();                              
            }
        }

        private void APITextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            APITextBlock.Visibility = Visibility.Visible;
            APITextBox.Visibility = Visibility.Collapsed;
        }

        private void APITextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                APITextBlock.Visibility = Visibility.Visible;
                APITextBox.Visibility = Visibility.Collapsed;
            }
        }


    }
}
