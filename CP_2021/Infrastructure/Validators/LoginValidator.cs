using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CP_2021.Infrastructure.Validators
{
    class LoginValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool flag;
            if (((string)value).Length < 4 || ((string)value).Length > 15)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            return new ValidationResult(flag, "Логин должен содержать от 4 до 15 символов");
        }
    }
}
