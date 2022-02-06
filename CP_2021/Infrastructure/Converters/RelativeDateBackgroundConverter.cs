using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CP_2021.Infrastructure.Converters
{
    internal class RelativeDateBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)
            {
                var date1 = ((DateTime)value).Date;
                var date2 = DateTime.Now.Date;
                if(((DateTime)value).Date <= DateTime.Now.Date.AddDays(3))
                {
                    return Brushes.LightSkyBlue;
                }
                
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
