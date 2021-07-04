using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Search
{
    interface ISearchManager<T>
    {
        void SetSearchStrategy(ISearchStrategy<T> searchStrategy);
        void ExecuteSearchStrategy();
    }
}
