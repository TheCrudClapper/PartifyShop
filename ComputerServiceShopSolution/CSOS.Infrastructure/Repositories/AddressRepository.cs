using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DatabaseContext _dbContext;
        public AddressRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public Task<Address?> GetAddress(int id)
        {
            return _dbContext.Addresses
                .FirstOrDefaultAsync(item => item.Id == id && item.IsActive);
        }

        public async Task<ApplicationUser?> GetUserWithAddress(Guid userId)
        {
            return  await _dbContext.Users
                .Where(item => item.IsActive && item.Id == userId)
                .Include(item => item.Address)
                .FirstOrDefaultAsync();
        }

    }
}
