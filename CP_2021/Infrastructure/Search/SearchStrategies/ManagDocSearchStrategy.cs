﻿using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class ManagDocSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.ManagDoc != null && task.Task.ManagDoc.ToLower().Contains(_searchString.ToLower());
        }

        public ManagDocSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
