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
    public class CountryRepository : ICountryRepository
    {
        private readonly DatabaseContext _dbContext;

        public CountryRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Country>> GetActiveCountriesAsync()
        {
            return await _dbContext.Countries
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}