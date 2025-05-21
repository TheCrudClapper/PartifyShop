using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.CartMappings
{
    public static class CartMappings
    {
        public static Cart ToCartEntity(this RegisterDto dto)
        {
            return new Cart()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
            };
        }
    }
}
