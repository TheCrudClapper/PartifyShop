using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductImageRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ProductImage>> GetImagesFromOfferAsync(int offerId)
        {
            return await _dbContext.Offers
                .Where(item => item.IsActive && item.Id == offerId)
                .Include(item => item.Product)
                    .ThenInclude(item => item.ProductImages)
                    .SelectMany(item => item.Product.ProductImages)
                    .Where(item => item.IsActive)
                    .ToListAsync();
        }
    }
}
