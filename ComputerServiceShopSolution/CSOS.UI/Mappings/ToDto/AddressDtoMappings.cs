using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO.AddressDto;
namespace CSOS.UI.Mappings.ToDto
{
    public static class AddressDtoMappings
    {
        public static AddressUpdateRequest ToAddressUpdateRequest(this EditAddressViewModel viewModel)
        {
            return new AddressUpdateRequest()
            {
                Id = viewModel.Id,
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
