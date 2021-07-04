using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search
{
    class SearchManager : ISearchManager<ProductionTask>
    {
        private ISearchStrategy<ProductionTask> _searchStrategy;

        public void ExecuteSearchStrategy()
        {
            _searchStrategy.ClearResults();
            _searchStrategy.Search();
        }

        public List<ProductionTask> GetSearchResults()
        {
            return _searchStrategy.GetSearchResults();
        }

        public void SetSearchStrategy(ISearchStrategy<ProductionTask> searchStrategy)
        {
            this._searchStrategy = searchStrategy;
        }
    }
}
