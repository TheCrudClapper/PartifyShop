using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.DTO.Responses.Addresses;

namespace CSOS.Core.DTO
{
    public  class AccountDetailsDto
    {
        public EditAddressResponseDto EditAddressResponseDto { get; set; } = null!;
        public AccountDto AccountDto { get; set; } = null!;
    }
}
