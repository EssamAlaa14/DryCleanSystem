using Domain.Entities;
using Domain.Entities.Orders;

namespace Application.Common.Interfaces.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories

        IRepository<CleaningType> CleaningTypes { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }

        #endregion

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
