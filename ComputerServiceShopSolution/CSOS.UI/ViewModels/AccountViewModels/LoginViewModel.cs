using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ComputerServiceOnlineShop.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        public bool isPersistent { get; set; }
    }
}
