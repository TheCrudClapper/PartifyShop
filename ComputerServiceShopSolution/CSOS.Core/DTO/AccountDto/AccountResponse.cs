namespace CSOS.Core.DTO.AccountDto
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? NIP { get; set; }
        public string? Title { get; set; }
    }
}

