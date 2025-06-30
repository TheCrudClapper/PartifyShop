using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.ToEntity.OfferMappings
{
    public static class OfferMapping
    {
        public static Offer ToOfferEntity(this AddOfferDto dto, Guid userId)
        {
            return new Offer()
            {
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                Price = dto.Price,
                SellerId = userId,
                StockQuantity = dto.StockQuantity,
                IsOfferPrivate = dto.IsOfferPrivate,
            };
        }
    }
}
