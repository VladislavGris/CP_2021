using CP_2021.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Repositories.Base
{
    class BaseRepository<T> : IRepository<T> where T : class
    {
        internal ApplicationContext context;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual void Delete(T entityToDelete)
        {
            if(context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(int id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual IEnumerable<T> Get()
        {
            return dbSet.ToList();
        }

        public virtual T GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void Refresh()
        {
            foreach (var entity in dbSet.ToList())
            {
                context.Entry(entity).Reload();
            }

        }

        public virtual void Update(T entityToUpdate)
        {
            //dbSet.Attach(entityToUpdate);
            //context.Entry(entityToUpdate).State = EntityState.Modified;
            dbSet.Update(entityToUpdate);
        }
    }
}
