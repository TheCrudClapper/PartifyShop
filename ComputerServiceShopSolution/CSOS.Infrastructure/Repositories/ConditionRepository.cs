using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;
using CSOS.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly DatabaseContext _dbContext;
        public ConditionRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public async Task<IEnumerable<Condition>> GetAllConditionsAsync()
        {
            return await _dbContext.Conditions
             .Where(item => item.IsActive).ToListAsync();
        }
    }
}
