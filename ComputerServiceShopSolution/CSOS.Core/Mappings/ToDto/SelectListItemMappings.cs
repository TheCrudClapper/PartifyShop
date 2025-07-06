using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.ToDto
{
    public static class SelectListItemMappings
    {
        public static SelectListItemDto ToSelectListItem(this DeliveryType deliveryType)
        {
            return new SelectListItemDto
            {
                Text = deliveryType.Title,
                Value = deliveryType.Id.ToString()
            };
        }
    }
}
