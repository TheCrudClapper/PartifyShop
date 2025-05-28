using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IAddressRepository
    {
        Task<ApplicationUser?> GetUserWithAddress(Guid userId);
        Task<Address?> GetAddress(int id);
    }
}
