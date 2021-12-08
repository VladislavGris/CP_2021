using CP_2021.Models.ProcedureResuts;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters
{
    internal class UserNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is UserNames)
            {
                return $"{((UserNames)value).Surname} {((UserNames)value).Name}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
