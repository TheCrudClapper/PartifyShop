using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Infrastructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductImageRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ProductImage>> GetImagesFromOffer(int offerId, List<string> imageUrls)
        {
            return await _dbContext.Offers
                .Where(item => item.IsActive && item.Id == offerId)
                .Include(item => item.Product)
                    .ThenInclude(item => item.ProductImages)
                    .SelectMany(item => item.Product.ProductImages)
                    .Where(item => imageUrls.Contains(item.ImagePath) && item.IsActive)
                    .ToListAsync();
        }
    }
}
