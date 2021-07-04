using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class IncDocSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.IncDoc != null && task.Task.IncDoc.ToLower().Contains(_searchString.ToLower());
        }

        public IncDocSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
