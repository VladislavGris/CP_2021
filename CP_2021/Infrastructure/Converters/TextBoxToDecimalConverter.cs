using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters
{
    class TextBoxToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal decimalValue;
            if (Decimal.TryParse(value.ToString(), out decimalValue))
            {
                return decimalValue;
            }
            else
            {
                return 0;
            }
        }
    }
}
