using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IOfferRepository 
    {
        Task AddAsync(Offer entity);
        Task RemoveAsync(int id);
        Task UpdateAsync(Offer entity, int id);
        
    }
}
