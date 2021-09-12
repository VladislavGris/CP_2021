using CP_2021.Data;
using CP_2021.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Repositories.Base
{
    class DbRepository<T> : IAsyncRepository<T> where T : Entity, new()
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbRepository(ApplicationContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public virtual IQueryable<T> Items => _set;



        public T Add(T entity)
        {
            if(entity != null)
            {
                _context.Entry(entity).State = EntityState.Added;
                if(AutoSaveChanges)
                    _context.SaveChanges();
            }
            return entity;
        }

        public async Task<T> AddAsync(T entity, CancellationToken token)
        {
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Added;
                if (AutoSaveChanges)
                    await _context.SaveChangesAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public T Get(Guid id) => Items.SingleOrDefault(x => x.Id == id);

        public async Task<T> GetAsync(Guid id, CancellationToken token = default) => 
            await Items.FirstOrDefaultAsync(x => x.Id == id, token).ConfigureAwait(false);

        public void Remove(T entity)
        {
            if(entity != null)
            {
                _context.Remove(entity);
                if (AutoSaveChanges)
                {
                    _context.SaveChanges();
                }
            }
        }

        public async Task RemoveAsync(T entity, CancellationToken token = default)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                if (AutoSaveChanges)
                    await _context.SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public void Remove(Guid id)
        {
            _context.Remove(new T { Id = id });
            if (AutoSaveChanges)
            {
                _context.SaveChanges();
            }
        }

        public async Task RemoveAsync(Guid id,  CancellationToken token = default)
        {
            _context.Remove(new T { Id = id });
            if (AutoSaveChanges)
                await _context.SaveChangesAsync(token).ConfigureAwait(false);
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Modified;
                if (AutoSaveChanges)
                    _context.SaveChanges();
            }
        }

        public async Task UpdateAsync(T entity, CancellationToken token = default)
        {
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Modified;
                if (AutoSaveChanges)
                    await _context.SaveChangesAsync(token).ConfigureAwait(false);
            }
        }
    }
}
