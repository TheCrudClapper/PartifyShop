using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AccountDtoMappings
    {
        public static RegisterDto ToDto(this RegisterViewModel ViewModel)
        {
            return new RegisterDto()
            {
                FirstName = ViewModel.FirstName,
                Surname = ViewModel.Surname,
                Email = ViewModel.Email,
                HouseNumber = ViewModel.HouseNumber,
                NIP = ViewModel.NIP,
                Password = ViewModel.Password,
                PhoneNumber = ViewModel.PhoneNumber,
                Place = ViewModel.Place,
                PostalCity = ViewModel.PostalCity,
                PostalCode = ViewModel.PostalCode,
                SelectedCountry = int.Parse(ViewModel.SelectedCountry),
                Street = ViewModel.Street,
                Title = ViewModel.Title,
            };
        }
        public static LoginDto ToDto(this LoginViewModel ViewModel)
        {
            return new LoginDto()
            {
                Email = ViewModel.Email,
                Password = ViewModel.Password,
            };
        }
    }
}
