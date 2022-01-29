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
                    return "Отработка документации";
                case 2:
                    return "ВК на склад";
                case 3:
                    return "В работе по кооперации";
                case 4:
                    return "В работе";
                case 5:
                    //return "Временная выдача";
                    return "Не задано";
                case 6:
                    //return "Проверка СКБ";
                    return "Не задано";
                case 7:
                    //return "Склад ОЭЦ";
                    return "Не задано";
                case 8:
                    //return "Собрано по акту";
                    return "Не задано";
                case 9:
                    return "Склад";
                case 10:
                    return "Документация отработана";
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
