using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.Entities;

namespace CSOS.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository contract for accessing address data.
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Asynchronously retrieves an <see cref="Address"/> by its ID from the data store.
        /// </summary>
        /// <param name="addressId">The ID of the address to search for.</param>
        /// <returns>The matching <see cref="Address"/> if found; otherwise, <c>null</c>.</returns>
        Task<Address?> GetAddressByIdAsync(int addressId);
    }
}
