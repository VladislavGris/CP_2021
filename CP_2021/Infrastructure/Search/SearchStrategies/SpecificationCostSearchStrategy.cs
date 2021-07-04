using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class SpecificationCostSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.SpecCost!=null && task.Task.SpecCost.ToLower().Equals(_searchString.ToLower());
        }

        public SpecificationCostSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
