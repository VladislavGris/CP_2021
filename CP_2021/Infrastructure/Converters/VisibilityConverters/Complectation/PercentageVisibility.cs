using CP_2021.Models.ProcedureResuts.Plan;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters.VisibilityConverters.Complectation
{
    class PercentageVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Task_Hierarchy_Formatting)
            {
                if (((Task_Hierarchy_Formatting)value).Percentage == null || ((Task_Hierarchy_Formatting)value).Percentage.Value == 0)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
