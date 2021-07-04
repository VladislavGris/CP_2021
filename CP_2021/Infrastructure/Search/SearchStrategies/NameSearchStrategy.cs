using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class NameSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.Name.ToLower().Contains(_searchString.ToLower());
        }

        public NameSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString){}
    }
}
