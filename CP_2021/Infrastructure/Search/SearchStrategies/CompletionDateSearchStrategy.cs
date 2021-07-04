using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class CompletionDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime readyDate;
            if(DateTime.TryParse(_searchString, out readyDate))
            {
                return task.Task.InProduction.CompletionDate != null && task.Task.InProduction.CompletionDate.Equals(readyDate);
            }
            return false;
        }

        public CompletionDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
