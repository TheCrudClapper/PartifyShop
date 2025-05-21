using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.ApplicationUserMappings
{
    public static class ApplicationUserMappings
    {
        public static ApplicationUser ToApplicationUserEntity (this RegisterDto dto, Address address, Cart cart)
        {
            return new ApplicationUser()
            {
                UserName = dto.Email,
                Address = address,
                FirstName = dto.FirstName,
                Title = dto.Title,
                Surname = dto.Surname,
                PhoneNumber = dto.PhoneNumber,
                Cart = cart,
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = dto.Email,
                NIP = dto.NIP,
            };
           
        }
    }
}
