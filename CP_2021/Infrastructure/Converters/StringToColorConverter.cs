using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CP_2021.Infrastructure.Converters
{
    internal class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString()));
            }
            return new SolidColorBrush(Color.FromRgb(0,0,0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ((Color)value).ToString();
            }
            return "#00000000";
        }
    }
}
