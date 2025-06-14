﻿using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModManager.ViewModel
{
    public partial class NavigationVM : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase? _currentView;

        public NavigationVM()
        {
            NavigateTo<ModListVM>();
        }

        public void NavigateTo<VM>() where VM : ViewModelBase
        {
            CurrentView = App.Current.ServiceProvider.GetService<VM>();
        }
    }
}
