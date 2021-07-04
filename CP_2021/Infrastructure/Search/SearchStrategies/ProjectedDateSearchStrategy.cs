using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class ProjectedDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime projDate;
            if(DateTime.TryParse(_searchString, out projDate))
            {
                return task.Task.InProduction.ProjectedDate != null && task.Task.InProduction.ProjectedDate.Equals(projDate);
            }
            return false;
        }

        public ProjectedDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
