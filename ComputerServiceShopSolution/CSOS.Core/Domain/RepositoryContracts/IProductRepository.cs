using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductRepository
    {
        Task AddAsync(Product entity);
        Task RemoveAsync(int id);
        Task UpdateAsync(Product entity, int id);
    }
}
