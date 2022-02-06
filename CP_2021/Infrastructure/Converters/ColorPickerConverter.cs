using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CP_2021.Infrastructure.Converters
{
    internal class ColorPickerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return (Color)ColorConverter.ConvertFromString(value.ToString());
            }
            return Colors.Black;
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
