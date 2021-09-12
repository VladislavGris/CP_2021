using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Repositories.Base
{
    interface IAsyncRepository<T> where T : Entity
    {
        IQueryable<T> Items { get; }

        T Get(Guid id);
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void Remove(Guid id);
    }
}
