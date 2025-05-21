using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IDeliveryTypeRepository
    {
        Task<List<DeliveryType>> GetActiveDeliveryTypesAsync();
    }
}