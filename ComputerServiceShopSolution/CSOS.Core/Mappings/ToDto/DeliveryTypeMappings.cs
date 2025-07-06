using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO.Responses.Deliveries;

namespace CSOS.Core.Mappings.ToDto
{
    public static class DeliveryTypeMappings
    {
        public static DeliveryTypeResponseDto ToDeliveryTypeResponseDto(this DeliveryType deliveryType)
        {
            return new DeliveryTypeResponseDto
            {

                Id = deliveryType.Id,
                Price = deliveryType.Price,
                Title = deliveryType.Title
            };
        }
    }
}
