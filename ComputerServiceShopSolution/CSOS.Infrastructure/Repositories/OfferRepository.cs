using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;

namespace CSOS.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DatabaseContext _dbContext;
        public OfferRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Offer entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public async Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Offer entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
