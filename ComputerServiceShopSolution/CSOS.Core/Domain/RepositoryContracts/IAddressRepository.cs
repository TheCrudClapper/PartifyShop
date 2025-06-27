using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IAddressRepository
    {
        
        /// <summary>
        /// Gets Addres Domain Model of given id from data store
        /// </summary>
        /// <param name="addressId">Addres to search for</param>
        /// <returns>Returns Addres Domain Model</returns>
        Task<Address?> GetAddressByIdAsync(int addressId);
    }
}
