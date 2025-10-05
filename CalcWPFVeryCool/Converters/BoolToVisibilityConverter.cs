using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CalcWPFVeryCool.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool boolValue;
            if (bool.TryParse(value.ToString(), out boolValue)) return boolValue ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try
            {
                var visibility = (Visibility)value;
                return visibility == Visibility.Visible;
            }
            catch { }
            return false;
        }
    }
}
