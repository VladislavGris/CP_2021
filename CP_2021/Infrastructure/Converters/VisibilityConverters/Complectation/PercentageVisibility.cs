﻿using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Converters.VisibilityConverters.Complectation
{
    class PercentageVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProductionTaskDB)
            {
                if (((ProductionTaskDB)value).Complectation.Percentage == null || ((ProductionTaskDB)value).Complectation.Percentage.Value == 0)
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