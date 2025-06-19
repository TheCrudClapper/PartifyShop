namespace CSOS.Core.DTO
{
    public class PasswordDto
    {
        public string CurrentPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
