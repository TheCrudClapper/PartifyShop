using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.AddressDto;
using CSOS.UI.Mappings.Universal;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class AddressViewModelMappings
    {
        public static UserAddressDetailsViewModel ToViewModel(this UserAddressDetailsResponseDto dto)
        {
            return new UserAddressDetailsViewModel()
            {
                Address = dto.Address,
                AddressId = dto.AddressId,
                CustomerName = dto.CustomerName,
                PhoneNumber = dto.PhoneNumber,
                PostalInfo = dto.PostalInfo,
            };
        }
        public static EditAddressViewModel ToViewModel(this AddressResponse dto)
        {
            return new EditAddressViewModel()
            {
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                Id = dto.Id,
                Place = dto.Place,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
            };
        }

        public static EditAddressViewModel ToEditAddressViewModel(this AddressResponse response)
        {
            return new EditAddressViewModel()
            {
                Id = response.Id,
                Street = response.Street,
                HouseNumber = response.HouseNumber,
                SelectedCountry = response.SelectedCountry.ToString(),
                Place = response.Place,
                PostalCity = response.PostalCity,
                PostalCode = response.PostalCode,
            };
        }
    }
}
