using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class PercentSearchStrategy : BaseTaskSearchStrategy
    {

        protected override bool FieldContainsString(ProductionTask task)
        {
            float percent;
            if(float.TryParse(_searchString, out percent))
            {
                return task.Task.Complectation.Percentage.Equals(percent);
            }
            return false;
        }

        public PercentSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
