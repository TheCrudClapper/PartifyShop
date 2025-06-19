using CSOS.Core.DTO.Responses.Account;
using CSOS.UI.ViewModels.AccountViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class AccountMappings
    {
        public static UserDetailsViewModel ToUserDetailsViewModel(this AccountDto dto)
        {
            if(dto == null)
                return new UserDetailsViewModel();

            return new UserDetailsViewModel()
            {
                FirstName = dto.FirstName,
                NIP = dto.NIP,
                PhoneNumber = dto.PhoneNumber,
                Surname = dto.Surname,
                Title = dto.Title,
            };
        }
    }
}
