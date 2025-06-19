using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using ComputerServiceOnlineShop.ViewModels.OrderViewModels;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.UI.Mappings.Universal;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class AddressViewModelMappings
    {
        public static UserAddressDetailsViewModel ToViewModel(this UserAddresDetailsResponseDto dto)
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
        public static EditAddressViewModel ToViewModel(this EditAddressResponseDto dto)
        {
            return new EditAddressViewModel()
            {
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                Id = dto.Id,
                Place = dto.Place,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                CountriesSelectionList = dto.CountriesSelectionList.ConvertToSelectListItem(),
                SelectedCountry = dto.SelectedCountry,
            };
        }
        
    }
}
