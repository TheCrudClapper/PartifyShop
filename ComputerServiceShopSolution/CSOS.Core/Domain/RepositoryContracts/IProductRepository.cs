using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductRepository
    {
        /// <summary>
        /// Adds product to database
        /// </summary>
        /// <param name="entity">Product to be added to db</param>
        /// <returns></returns>
        Task AddAsync(Product entity);
    }
}
