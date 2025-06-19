namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? NIP { get; set; }
        public string? Title { get; set; }
    }
}
