using System;
using System.Collections.Generic;

namespace CP_2021.Infrastructure.Search
{
    interface ISearchStrategy<T>
    {
        void ClearResults();
        void Search();
        List<T> GetSearchResults();
    }
}
