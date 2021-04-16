using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Repositories.Base
{
    interface IRepository<T> where T:class
    {
        void Delete(T entityToDelete);
        void Delete(int id);
        IEnumerable<T> Get();
        T GetByID(int id);
        IEnumerable<T> GetWithRawSql(string query, params object[] parameters);
        void Insert(T entity);
        void Update(T entityToUpdate);
    }
}
