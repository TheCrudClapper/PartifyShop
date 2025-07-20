using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using CSOS.Core.Domain.Entities;
using CSOS.Infrastructure.DbContext;

namespace CSOS.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DatabaseContext _dbContext;
        public AddressRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public Task<Address?> GetAddressByIdAsync(int id)
        {
            return _dbContext.Addresses
                .FirstOrDefaultAsync(item => item.Id == id && item.IsActive);
        }

        public async Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId)
        {
            return  await _dbContext.Users
                .Where(item => item.IsActive && item.Id == userId)
                .Include(item => item.Address)
                .FirstOrDefaultAsync();
        }

    }
}
