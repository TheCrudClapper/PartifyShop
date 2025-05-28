namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    }
}
