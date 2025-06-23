using System.ComponentModel.DataAnnotations;

namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class UserDetailsViewModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(10)]
        public string? NIP { get; set; }

        [StringLength(4)]
        public string? Title { get; set; }
    }
}
