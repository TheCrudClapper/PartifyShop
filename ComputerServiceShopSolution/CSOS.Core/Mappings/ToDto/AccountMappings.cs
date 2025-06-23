using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.DTO.Responses.Addresses;

namespace CSOS.Core.Mappings.ToDto
{
    public static class AccountMappings
    {
        public static AccountDto ToAccountResponseDto(this ApplicationUser account)
        {
            return new AccountDto()
            {
                FirstName = account.FirstName,
                NIP = account.NIP,
                PhoneNumber = account.PhoneNumber,
                Surname = account.Surname,
                Title = account.Title,
            };
        }

        public static UserAddresDetailsResponseDto ToUserAddresDetailsResponse(this ApplicationUser account)
        {
            return new UserAddresDetailsResponseDto()
            {
                CustomerName = account.FirstName + " " + account.Surname,
                PostalInfo = account.Address.PostalCode + " " + account.Address.PostalCity,
                AddressId = account.AdressId,
                Address = account.Address.Place + ",  " + account.Address.Street + " " + account.Address.HouseNumber,
                PhoneNumber = account?.PhoneNumber,
            };
        }

        public static EditAddressResponseDto ToEditAddresResponse(this ApplicationUser account)
        {
            return new EditAddressResponseDto()
            {
                Id = account.AdressId,
                HouseNumber = account.Address.HouseNumber,
                Place = account.Address.Place,
                PostalCity = account.Address.PostalCity,
                PostalCode = account.Address.PostalCode,
                Street = account.Address.Street,
                SelectedCountry = account.Address.CountryId.ToString(),
            };
        }
    }
}
