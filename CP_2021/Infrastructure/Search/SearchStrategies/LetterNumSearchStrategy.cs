using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class LetterNumSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            return task.Task.Manufacture.LetterNum != null && task.Task.Manufacture.LetterNum.ToLower().Contains(_searchString.ToLower());
        }

        public LetterNumSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
