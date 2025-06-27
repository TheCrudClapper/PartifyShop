using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IConditionRepository
    {
        /// <summary>
        /// Gets all product conditions from db
        /// </summary>
        /// <returns>Return an List of Condition Domain Models</returns>
        Task<List<Condition>> GetAllConditionsAsync();
    }
}
