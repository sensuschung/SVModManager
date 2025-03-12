using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace SVModManager.Converters
{

    public class BooleanConverter<T> : IValueConverter
    {
        protected BooleanConverter(T tValue, T fValue)
        {
            True = tValue;
            False = fValue;
        }

        public T True { get; set; }

        public T False { get; set; }


        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool flag && flag ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T flag && EqualityComparer<T>.Default.Equals(flag, True);
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InverseBooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
