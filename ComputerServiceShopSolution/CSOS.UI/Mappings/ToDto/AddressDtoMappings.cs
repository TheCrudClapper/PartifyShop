using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AddressDtoMappings
    {
        public static AddressDto ToDto(this EditAddressViewModel viewModel)
        {
            return new AddressDto()
            {
                CountryId = int.Parse(viewModel.SelectedCountry),
                HouseNumber = viewModel.HouseNumber,
                Place = viewModel.Place,
                PostalCity = viewModel.PostalCity,
                PostalCode = viewModel.PostalCode,
                Street = viewModel.Street,
            };
        }
    }
}
