using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetAllProductCategories();
    }
}
