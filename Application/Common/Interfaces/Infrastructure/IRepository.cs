using Domain.Entities.Base;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Infrastructure
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetByPredicateAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void SoftDelete(T entity);
        void SoftDeleteRange(IEnumerable<T> entities);

        void HardDelete(T entity);
        void HardDeleteRange(IEnumerable<T> entities);
    }
}
