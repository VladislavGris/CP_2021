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
    class RealDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime realDate;
            if(DateTime.TryParse(_searchString, out realDate))
            {
                return task.Task.RealDate != null && task.Task.RealDate.Equals(realDate);
            }
            throw new IncorrectDateFormatException(DateError);
        }

        public RealDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
