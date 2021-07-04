using Common.Wpf.Data;
using CP_2021.Infrastructure.Exceptions;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class GivingDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime givingDate;
            if(DateTime.TryParse(_searchString, out givingDate))
            {
                return task.Task.InProduction.GivingDate != null && task.Task.InProduction.GivingDate.Equals(givingDate);
            }
            throw new IncorrectDateFormatException(DateError);
        }

        public GivingDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
