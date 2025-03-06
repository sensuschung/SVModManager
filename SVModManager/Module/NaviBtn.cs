using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SVModManager.Module
{
    public class NaviBtn : RadioButton
    {
        static NaviBtn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NaviBtn), new FrameworkPropertyMetadata(typeof(NaviBtn)));
        }
    }
}
