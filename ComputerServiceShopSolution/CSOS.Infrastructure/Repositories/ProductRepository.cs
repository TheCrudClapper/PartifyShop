using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;

namespace CSOS.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Product entity)
        {
            await _dbContext.Products.AddAsync(entity);
        }
    }
}
