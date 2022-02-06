using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CP_2021.Infrastructure.Validators
{
    internal class NoteLengthValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string note = ((value as BindingExpression).BindingGroup.Items[0] as ProductionTask)?.data?.Note;
            if(note.Length > 300)
            {
                return new ValidationResult(false, "Количество символов примечания превысило 300 символов");
            }
            return ValidationResult.ValidResult;
        }
    }
}
