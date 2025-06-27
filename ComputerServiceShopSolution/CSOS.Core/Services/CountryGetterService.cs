using ComputerServiceOnlineShop.Abstractions;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;

namespace ComputerServiceOnlineShop.Services
{
    public class CountryGetterService : ICountriesService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryGetterService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<SelectListItemDto>> GetCountriesSelectionList()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();

            return countries.Select(item => new SelectListItemDto
            {
                Text = item.CountryName,
                Value = item.Id.ToString()
            })
            .ToList();
        }
    }
}