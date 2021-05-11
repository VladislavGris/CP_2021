using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters
{
    class CompletionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((short)value)
            {
                case 0:
                    return "Не задано";
                case 1:
                    return "Склад";
                case 2:
                    return "Отработка документации";
                case 3:
                    return "ВК на склад";
                case 4:
                    return "В работе";
                case 5:
                    return "Формирование актов";
                case 6:
                    return "Склад ОЭЦ";
                default:
                    return "Не задано";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
