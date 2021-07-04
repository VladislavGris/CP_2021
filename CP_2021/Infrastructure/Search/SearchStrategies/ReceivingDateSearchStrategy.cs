using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class ReceivingDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime receivingDate;
            if(DateTime.TryParse(_searchString, out receivingDate))
            {
                return task.Task.Giving.ReceivingDate != null && task.Task.Giving.ReceivingDate.Equals(receivingDate);
            }
            return false;
        }

        public ReceivingDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
