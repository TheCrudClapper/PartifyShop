using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.DTO.Responses.CartItem;

namespace CSOS.Core.Mappings.ToDto
{
    public static class CartResponseDtoMappings
    {
        public static CartResponseDto ToCartResponseDto(this Cart cart)
        {
            return new CartResponseDto()
            {
                CartItems = cart.CartItems.Where(item => item.IsActive)
                    .Select(item => new CartItemResponseDto
                    {
                        Category = item.Offer.Product.ProductCategory.Name,
                        Condition = item.Offer.Product.Condition.ConditionTitle,
                        DateAdded = item.DateCreated,
                        Id = item.Id,
                        Price = item.Offer.Price,
                        Quantity = item.Quantity,
                        Title = item.Offer.Product.ProductName,
                        ImageUrl = item.Offer.Product.ProductImages.FirstOrDefault()?.ImagePath,
                        OfferId = item.OfferId,

                    }).ToList(),
                TotalCartValue = cart.TotalCartValue ?? 0,
                TotalDeliveryValue = cart.MinimalDeliveryValue ?? 0,
                TotalItemsValue = cart.TotalItemsValue ?? 0,
            };
        }
    }
}
