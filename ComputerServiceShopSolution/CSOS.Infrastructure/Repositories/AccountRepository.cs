using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext _dbContext;
        public AccountRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task Add(ApplicationUser entity)
        {
            await _dbContext.Users.AddAsync(entity);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(item => item.IsActive && item.Id == id);
        }

        public async Task<bool> IsUserByEmailInDatabaseAsync(string Email)
        {
            return await _dbContext.Users
                 .AnyAsync(item => item.UserName == Email && item.IsActive);
        }

    }
}
