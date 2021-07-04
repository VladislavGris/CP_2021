using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search.SearchStrategies
{
    class BaseTaskSearchStrategy : ISearchStrategy<ProductionTask>
    {
        private List<ProductionTask> _searchResults;
        protected string _searchString;
        private TreeGridModel _searchSource;

        public void ClearResults()
        {
            _searchResults = new List<ProductionTask>();
        }

        public List<ProductionTask> GetSearchResults() => _searchResults;

        public void Search()
        {
            foreach(ProductionTask root in _searchSource)
            {
                CheckTask(root);
            }
        }

        private void CheckTask(ProductionTask task)
        {
            if (FieldContainsString(task))
            {
                _searchResults.Add(task);
            }
            if (task.HasChildren)
            {
                SearchInAllChildren(task);
            }
        }

        private void SearchInAllChildren(ProductionTask task)
        {
            foreach (ProductionTask child in task.Children)
            {
                CheckTask(child);
            }
        }

        protected virtual bool FieldContainsString(ProductionTask task)
        {
            return true;
        }

        protected BaseTaskSearchStrategy(TreeGridModel source, string searchString)
        {
            _searchSource = source;
            _searchString = searchString;
        }
    }
}
