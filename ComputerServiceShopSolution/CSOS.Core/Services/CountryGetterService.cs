using ComputerServiceOnlineShop.Abstractions;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.Mappings.ToDto;

namespace ComputerServiceOnlineShop.Services
{
    public class CountryGetterService : ICountriesGetterService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryGetterService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetCountriesSelectionList()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();

            return countries.Select(item => item.ToSelectListItem());
        }
    }
}