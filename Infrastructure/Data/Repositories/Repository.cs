using Application.Common.Exceptions;
using Application.Common.Interfaces.Infrastructure;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Get Methods
        public async Task<T> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            return entity ?? throw new NotFoundException(typeof(T).Name, id);
        }

        public async Task<T> GetByIdIncludeStringAsync(
            Guid id,
            CancellationToken cancellationToken = default,
            params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }
            }

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            return entity ?? throw new NotFoundException(typeof(T).Name, id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetByPredicateAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(predicate).ToListAsync(cancellationToken);
        }

        // Add Methods
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        // Update Methods
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        // Delete Methods
        public void SoftDelete(T entity)
        {
            entity.SoftDelete();
            _dbSet.Update(entity);
        }

        public void SoftDeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.SoftDelete();
            }

            _dbSet.UpdateRange(entities);
        }

        public void HardDelete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void HardDeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
