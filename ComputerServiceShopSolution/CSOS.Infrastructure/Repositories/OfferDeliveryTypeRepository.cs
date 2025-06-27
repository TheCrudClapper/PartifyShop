using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;

namespace CSOS.Infrastructure.Repositories
{
    public class OfferDeliveryTypeRepository : IOfferDeliveryTypeRepository
    {
        private readonly DatabaseContext _dbContext;
        public OfferDeliveryTypeRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(OfferDeliveryType entity)
        {
            await _dbContext.OfferDeliveryTypes.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<OfferDeliveryType> entities)
        {
            await _dbContext.OfferDeliveryTypes.AddRangeAsync(entities);
        }

    }
}
