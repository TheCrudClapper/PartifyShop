using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductCategoryRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public async Task<List<ProductCategory>> GetAllProductCategoriesAsync()
        {
            return await _dbContext.ProductCategories
                .Where(item => item.IsActive)
                .ToListAsync();
        }
    }
}
