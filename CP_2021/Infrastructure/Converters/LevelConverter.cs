using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters
{
    class LevelConverter : IValueConverter
    {
        public GridLength LevelWidth { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value * LevelWidth.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
