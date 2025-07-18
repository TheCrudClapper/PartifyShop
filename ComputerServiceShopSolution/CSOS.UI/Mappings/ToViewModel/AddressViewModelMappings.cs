﻿using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO.AddressDto;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class AddressViewModelMappings
    {
        public static UserAddressDetailsViewModel ToUserAddressDetailsViewModel(this UserAddressDetailsResponseDto dto)
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
