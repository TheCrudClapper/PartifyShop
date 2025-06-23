using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Account;
using CSOS.UI.ViewModels.AccountViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AccountDtoMappings
    {
        public static RegisterDto ToRegisterDto(this RegisterViewModel viewModel)
        {
            return new RegisterDto()
            {
                FirstName = viewModel.FirstName,
                Surname = viewModel.Surname,
                Email = viewModel.Email,
                HouseNumber = viewModel.HouseNumber,
                NIP = viewModel.NIP,
                Password = viewModel.Password,
                PhoneNumber = viewModel.PhoneNumber,
                Place = viewModel.Place,
                PostalCity = viewModel.PostalCity,
                PostalCode = viewModel.PostalCode,
                SelectedCountry = int.Parse(viewModel.SelectedCountry),
                Street = viewModel.Street,
                Title = viewModel.Title,
            };
        }
        public static LoginDto ToLoginDto(this LoginViewModel viewModel)
        {
            return new LoginDto()
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                isPersistent = viewModel.isPersistent,
            };
        }

        public static AccountDto ToAccountDto(this UserDetailsViewModel viewModel)
        {
            return new AccountDto()
            {
                FirstName = viewModel.FirstName,
                Surname = viewModel.Surname,
                NIP = viewModel.NIP,
                PhoneNumber = viewModel.PhoneNumber,
                Title = viewModel.Title,
            };
        }
    }
}
