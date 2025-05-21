using ComputerServiceOnlineShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductRepository
    {
        Task AddAsync(Product entity);
        Task RemoveAsync(int id);
        Task UpdateAsync(Product entity, int id);
    }
}
