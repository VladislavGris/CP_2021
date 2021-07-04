using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class CountSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            int count;
            if(Int32.TryParse(_searchString, out count))
            {
                return task.Task.Count.Equals(count);
            }
            return false;
        }

        public CountSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
