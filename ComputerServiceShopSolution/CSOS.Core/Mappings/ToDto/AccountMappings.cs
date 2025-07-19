using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.AddressDto;

namespace CSOS.Core.Mappings.ToDto
{
    public static class AccountMappings
    {
        public static AccountResponse ToAccountResponse(this ApplicationUser account)
        {
            return new AccountResponse()
            {
                FirstName = account.FirstName,
                NIP = account.NIP,
                PhoneNumber = account.PhoneNumber,
                Surname = account.Surname,
                Title = account.Title,
            };
        }
        public static UserAddressDetailsResponseDto ToUserAddresDetailsResponse(this ApplicationUser account)
        {
            return new UserAddressDetailsResponseDto()
            {
                CustomerName = account.FirstName + " " + account.Surname,
                PostalInfo = account.Address.PostalCode + " " + account.Address.PostalCity,
                AddressId = account.AdressId,
                Address = account.Address.Place + ",  " + account.Address.Street + " " + account.Address.HouseNumber,
                PhoneNumber = account?.PhoneNumber,
            };
        }

        public static AddressResponse ToAddressResponse(this ApplicationUser account)
        {
            return new AddressResponse()
            {
                Id = account.AdressId,
                HouseNumber = account.Address.HouseNumber,
                Place = account.Address.Place,
                PostalCity = account.Address.PostalCity,
                PostalCode = account.Address.PostalCode,
                Street = account.Address.Street,
                SelectedCountry = account.Address.CountryId,
            };
        }
    }
}
