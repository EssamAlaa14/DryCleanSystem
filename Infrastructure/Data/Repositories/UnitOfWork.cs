using Application.Common.Interfaces.Infrastructure;
using Domain.Entities;
using Domain.Entities.Orders;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork(
        ApplicationDbContext context
        ) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        private bool _disposed;

        #region Repositories

        private IRepository<CleaningType>? _cleaningTypes;
        public IRepository<CleaningType> CleaningTypes => _cleaningTypes ??= new Repository<CleaningType>(_context);
        
        private IRepository<Customer>? _customers;
        public IRepository<Customer> Customers => _customers ??= new Repository<Customer>(_context);

        private IRepository<Order>? _orders;
        public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);

        private IRepository<OrderItem>? _orderItems;
        public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);

        #endregion

        public async Task<int> CommitAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return new EfCoreTransaction(dbContextTransaction);
        }

        // Dispose Pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _context.Dispose();
                }

                // Note: No unmanaged resources to clean up in this class
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this); // Suppresses finalization for improved performance
        }
    }
}
