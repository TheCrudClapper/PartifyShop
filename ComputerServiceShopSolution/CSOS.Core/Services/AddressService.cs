using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Services
{
    public class AddressService : IAddressService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IAccountService _accountService;
        private readonly ICountriesService _countriesService;
        public AddressService(DatabaseContext databaseContext, IAccountService accountService, ICountriesService countriesService)
        {
            _databaseContext = databaseContext;
            _accountService = accountService;
            _countriesService = countriesService;
        }

        public async Task Edit(int id, AddressDto dto)
        {
            var address = await _databaseContext.Addresses.FirstAsync(item => item.Id == id);
            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            address.CountryId = dto.CountryId;
            address.DateEdited = DateTime.Now;
            address.Place = dto.Place;
            address.PostalCity = dto.PostalCity;
            address.PostalCode = dto.PostalCode;
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<EditAddressResponseDto> GetAddressForEdit()
        {
            var userId = _accountService.GetLoggedUserId();
            return await _databaseContext.Users.Where(item => item.IsActive && item.Id == userId)
                .Include(item => item.Address)
                .Select(item => new EditAddressResponseDto()
                {
                    Id = item.AdressId,
                    HouseNumber = item.Address.HouseNumber,
                    Place = item.Address.Place,
                    PostalCity = item.Address.PostalCity,
                    PostalCode = item.Address.PostalCode,
                    Street = item.Address.Street,
                    SelectedCountry = item.Address.CountryId.ToString(),
                }).FirstAsync();
        }

        public async Task<UserAddresDetailsResponseDto> GetUserAddresInfo()
        {
            var userId = _accountService.GetLoggedUserId();
            return await _databaseContext.Users.Where(item => item.IsActive && item.Id == userId)
                .Include(item => item.Address)
                .Select(item => new UserAddresDetailsResponseDto()
                {
                    CustomerName = item.FirstName + " " + item.Surname,
                    PostalInfo = item.Address.PostalCode + " " + item.Address.PostalCity,
                    AddressId = item.AdressId,
                    Address = item.Address.Place + " " + item.Address.HouseNumber,
                    PhoneNumber = item.PhoneNumber,
                })
                .FirstAsync();
        }
    }
}
