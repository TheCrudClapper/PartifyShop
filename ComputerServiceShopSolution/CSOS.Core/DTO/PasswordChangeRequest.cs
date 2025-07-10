namespace CSOS.Core.DTO
{
    public class PasswordChangeRequest
    {
        public string CurrentPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;

    }
}
