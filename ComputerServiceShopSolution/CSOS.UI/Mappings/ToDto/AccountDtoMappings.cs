using CSOS.Core.DTO.AccountDto;
using CSOS.UI.ViewModels.AccountViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class AccountDtoMappings
    {
        public static RegisterRequest ToRegisterRequest(this RegisterViewModel viewModel)
        {
            return new RegisterRequest()
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
        public static LoginRequest ToLoginRequest(this LoginViewModel viewModel)
        {
            return new LoginRequest()
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                IsPersistent = viewModel.IsPersistent,
            };
        }

        public static AccountUpdateRequest ToAccountUpdateRequest(this UserDetailsViewModel viewModel)
        {
            return new AccountUpdateRequest()
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
