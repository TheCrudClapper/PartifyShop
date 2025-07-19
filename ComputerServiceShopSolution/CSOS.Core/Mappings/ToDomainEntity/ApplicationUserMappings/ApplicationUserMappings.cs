using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AccountDto;

namespace CSOS.Core.Mappings.ToDomainEntity.ApplicationUserMappings
{
    public static class ApplicationUserMappings
    {
        public static ApplicationUser ToApplicationUserEntity (this RegisterRequest request, Address address, Cart cart)
        {
            return new ApplicationUser()
            {
                UserName = request.Email,
                Address = address,
                FirstName = request.FirstName,
                Title = request.Title,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Cart = cart,
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = request.Email,
                NIP = request.NIP,
            };
           
        }
    }
}
