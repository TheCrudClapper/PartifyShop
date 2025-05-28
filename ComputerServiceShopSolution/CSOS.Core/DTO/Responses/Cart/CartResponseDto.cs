using CSOS.Core.DTO.Responses.CartItem;

namespace CSOS.Core.DTO.Responses.Cart
{
    public class CartResponseDto
    {
        public IEnumerable<CartItemResponseDto> CartItems { get; set; } = new List<CartItemResponseDto>();
        public decimal? TotalCartValue { get; set; }
        public decimal? TotalDeliveryValue { get; set; }
        public decimal? TotalItemsValue { get; set; }
    }
}
