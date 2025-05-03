using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(T entity)
        {
            _dbSet.Update(entity); // Mark the entity as modified
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
