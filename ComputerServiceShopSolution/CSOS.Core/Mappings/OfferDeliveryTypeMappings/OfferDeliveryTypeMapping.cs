using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.OfferDeliveryTypeMappings
{
    public static class OfferDeliveryTypeMapping
    {
        //For mapping single parcel locker delivery
        public static OfferDeliveryType ToEntity(this AddOfferDto dto, Offer offer)
        {
            return new OfferDeliveryType()
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                Offer = offer,
                DeliveryTypeId = dto.SelectedParcelLocker!.Value,
            };
        }

        public static OfferDeliveryType ToEntity(this EditOfferDto dto, Offer offer)
        {
            return new OfferDeliveryType()
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                Offer = offer,
                DeliveryTypeId = dto.SelectedParcelLocker!.Value,
            };
        }

        public static OfferDeliveryType ToEntity(this int deliveryId, Offer offer)
        {
            return new OfferDeliveryType()
            {
                DeliveryTypeId = deliveryId,
                Offer = offer,
                IsActive = true,
                DateCreated = DateTime.Now,
            };
        }
    }
}
