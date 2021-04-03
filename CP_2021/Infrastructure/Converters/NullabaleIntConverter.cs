using System;
using System.Globalization;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters
{
    class NullabaleIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                if (((string)value).Equals(""))
                    return null;
                else
                    return (int)value;
            }
            throw new NotImplementedException();
        }
    }
}
