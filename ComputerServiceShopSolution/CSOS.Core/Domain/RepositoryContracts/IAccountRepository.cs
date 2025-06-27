using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Adds user to data storage
        /// </summary>
        /// <param name="entity">Represents Domain Model</param>
        /// <returns>Return Application User Domain Model</returns>
        Task AddAsync(ApplicationUser entity);
        
        /// <summary>
        /// Check whether user of given email exist in database
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Returns boolean value</returns>
        Task<bool> IsUserByEmailInDatabaseAsync(string email);

        /// <summary>
        /// Gets user based from database based on his Id
        /// </summary>
        /// <param name="userId">User id to search for</param>
        /// <returns>Return Application User Domain Model</returns>
        Task<ApplicationUser?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Gets ApplicationUser Domain Model with eager loaded address by user Id
        /// </summary>
        /// <param name="userId">Id of an user</param>
        /// <returns>Returns ApplicationUser Domain Model</returns>
        Task<ApplicationUser?> GetUserWithAddressAsync(Guid userId);
    }
}
