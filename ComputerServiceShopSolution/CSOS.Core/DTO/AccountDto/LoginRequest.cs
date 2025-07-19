namespace CSOS.Core.DTO.AccountDto
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsPersistent { get; set; }
    }
}
