using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IDeliveryTypeRepository
    {
        /// <summary>
        /// Gets all active delivery types from db
        /// </summary>
        /// <returns>Returns an List of Delivery Type Domain Models</returns>
        Task<List<DeliveryType>> GetAllDeliveryTypesAsync();
    }
}