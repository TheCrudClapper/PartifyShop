using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductCategoryRepository
    {
        /// <summary>
        /// Gets active all product categories from databse
        /// </summary>
        /// <returns>Returns an list of Product Category Domain Models</returns>
        Task<List<ProductCategory>> GetAllProductCategoriesAsync();
    }
}
