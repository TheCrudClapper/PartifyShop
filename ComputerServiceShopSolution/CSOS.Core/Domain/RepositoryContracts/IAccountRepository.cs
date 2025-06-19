using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        Task Add(ApplicationUser entity);
        Task<bool> IsUserByEmailInDatabaseAsync(string Email);
        Task<ApplicationUser?> GetUserByIdAsync(Guid id);
    }
}
