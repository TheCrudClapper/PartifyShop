using ComputerServiceOnlineShop.Entities.Models;


namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Gets an List of all countries from db
        /// </summary>
        /// <returns>Return an List of Country Domain Models</returns>
        Task<List<Country>> GetAllCountriesAsync();
    }
}
