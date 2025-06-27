using CSOS.Core.Domain.RepositoryContracts;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class DeliveryTypeRepository : IDeliveryTypeRepository
    {
        private readonly DatabaseContext _dbContext;

        public DeliveryTypeRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DeliveryType>> GetAllDeliveryTypesAsync()
        {
            return await _dbContext.DeliveryTypes
                .Where(dt => dt.IsActive)
                .ToListAsync();
        }
    }
}