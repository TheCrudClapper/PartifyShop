using ComputerServiceOnlineShop.ViewModels.CartItemViewModels;
using ComputerServiceOnlineShop.ViewModels.CartViewModels;
using CSOS.Core.DTO.Responses.Cart;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class CartViewModelMappings
    {
        public static CartViewModel ToViewModel(this CartResponseDto dto)
        {
            return new CartViewModel()
            {
                TotalItemsValue = dto.TotalItemsValue,
                TotalCartValue = dto.TotalCartValue,
                TotalDeliveryValue = dto.TotalDeliveryValue,
                CartItems = dto.CartItems.Select(item => new CartItemViewModel()
                {
                    Category = item.Category,
                    Condition = item.Condition,
                    DateAdded = item.DateAdded,
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    OfferId = item.OfferId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Title = item.Title
                }).ToList(),
            };
        }
    }
}
