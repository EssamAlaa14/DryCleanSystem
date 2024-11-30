namespace Application.Common.Interfaces.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
