using Common.Wpf.Data;
using CP_2021.Infrastructure.Exceptions;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class VishDateSearchStrategy : BaseTaskSearchStrategy
    {
        protected override bool FieldContainsString(ProductionTask task)
        {
            DateTime vishDate;
            if(DateTime.TryParse(_searchString, out vishDate))
            {
                return task.Task.VishDate != null && task.Task.VishDate.Equals(vishDate);
            }
            throw new IncorrectDateFormatException(DateError);
        }

        public VishDateSearchStrategy(TreeGridModel source, string searchString) : base(source, searchString) { }
    }
}
