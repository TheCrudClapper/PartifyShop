using ComputerServiceOnlineShop.Entities.Models;


namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetActiveCountriesAsync();
    }
}
