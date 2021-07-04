using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class ManufactureSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.Manufacture.Name != null && task.Task.Manufacture.Name.ToLower().Contains(_searchString.ToLower());
        }

        public ManufactureSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
