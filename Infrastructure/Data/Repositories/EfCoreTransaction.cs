using Application.Common.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data.Repositories
{
    public class EfCoreTransaction(
        IDbContextTransaction dbContextTransaction
        ) : ITransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction = dbContextTransaction;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }
    }
}
