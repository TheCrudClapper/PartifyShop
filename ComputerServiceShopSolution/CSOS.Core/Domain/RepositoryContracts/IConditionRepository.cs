using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IConditionRepository
    {
        Task<List<Condition>> GetAllProductConditions();
    }
}
