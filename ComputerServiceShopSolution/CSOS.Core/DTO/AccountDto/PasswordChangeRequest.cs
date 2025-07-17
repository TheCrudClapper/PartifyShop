namespace CSOS.Core.DTO.AccountDto
{
    public class PasswordChangeRequest
    {
        public string CurrentPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;

    }
}
