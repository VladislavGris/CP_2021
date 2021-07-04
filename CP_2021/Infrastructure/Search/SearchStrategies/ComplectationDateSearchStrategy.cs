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
    class ComplectationDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime compDate;
            if(DateTime.TryParse(_searchString, out compDate))
            {
                return task.Task.Complectation.ComplectationDate != null && task.Task.Complectation.ComplectationDate.Equals(compDate);
            }
            throw new IncorrectDateFormatException(DateError);
        }

        public ComplectationDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
