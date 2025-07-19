using CSOS.Core.DTO;
using System.ComponentModel.DataAnnotations;
using CSOS.Core.DTO.AccountDto;

namespace CSOS.UI.ViewModels.AccountViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Enter your current password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Enter your new password")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password don't match.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Enter confirmation password")]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }
    public static class PasswordChangeViewModelMapping
    {
        public static PasswordChangeRequest ToPasswordChangeRequest(this PasswordChangeViewModel viewModel)
        {
            return new PasswordChangeRequest()
            {
                CurrentPassword = viewModel.CurrentPassword,
                ConfirmPassword = viewModel.ConfirmPassword,
                NewPassword = viewModel.NewPassword,
            };
        }
    }
}
