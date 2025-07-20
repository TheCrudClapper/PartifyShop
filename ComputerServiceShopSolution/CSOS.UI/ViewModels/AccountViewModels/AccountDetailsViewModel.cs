using CSOS.UI.ViewModels.AddressViewModels;

namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class AccountDetailsViewModel
    {
        public UserDetailsViewModel UserDetails { get; set; } = null!;
        public EditAddressViewModel EditAddress { get; set; } = null!;
        public PasswordChangeViewModel PasswordChange { get; set; } = null!;
    }
}
