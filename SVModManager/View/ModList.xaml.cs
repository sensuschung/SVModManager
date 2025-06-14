﻿using System;
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
using Microsoft.Extensions.DependencyInjection;
using SVModManager.ViewModel;

namespace SVModManager.View
{
    /// <summary>
    /// ModList.xaml 的交互逻辑
    /// </summary>
    public partial class ModList : UserControl
    {
        public ModList()
        {
            InitializeComponent();
            DataContext = App.Current.ServiceProvider.GetRequiredService<ModListVM>();
        }
    }
}
