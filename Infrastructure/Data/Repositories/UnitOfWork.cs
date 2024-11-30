using Application.Common.Interfaces.Infrastructure;
using Domain.Entities;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork(
        ApplicationDbContext context
        ) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        private bool _disposed;

        #region Repositories

        private IRepository<Customer>? _customers;
        public IRepository<Customer> Customers => _customers ??= new Repository<Customer>(_context);

        #endregion

        public async Task<int> CommitAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);

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
