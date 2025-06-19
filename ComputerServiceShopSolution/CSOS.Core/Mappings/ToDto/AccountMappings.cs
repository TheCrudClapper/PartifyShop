using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO.Responses.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.Mappings.ToDto
{
    public static class AccountMappings
    {
        public static AccountDto ToAccountResponseDto(this ApplicationUser account)
        {
            return new AccountDto()
            {
                FirstName = account.FirstName,
                NIP = account.NIP,
                PhoneNumber = account.PhoneNumber,
                Surname = account.Surname,
                Title  = account.Title,
            };
        } 
    }
}
