using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IOfferDeliveryTypeRepository
    {
        /// <summary>
        /// Adds to db entity
        /// </summary>
        /// <param name="entity">Entity to be added to database</param>
        /// <returns></returns>
        Task AddAsync(OfferDeliveryType entity);

        /// <summary>
        /// Adds a range of items (IEnumerable) to db
        /// </summary>
        /// <param name="entities">List of items to be added to db</param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities);
    }
}
