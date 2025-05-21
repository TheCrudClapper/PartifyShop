using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IOfferDeliveryTypeRepository
    {
        Task AddAsync(OfferDeliveryType entity);
        Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities);
        Task RemoveAsync(int id);
        Task UpdateAsync(OfferDeliveryType entity, int id);
    }
}
