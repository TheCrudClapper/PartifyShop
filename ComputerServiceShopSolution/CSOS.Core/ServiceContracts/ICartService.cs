using CSOS.Core.DTO.Responses.Cart;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface ICartService
    {
        Task AddToCart(int offerId, int quantity = 1);
        Task DeleteFromCart(int id);
        Task<CartResponseDto> GetLoggedUserCart();
        Task<int> GetLoggedUserCartId();
        Task UpdateTotalCartValue(int id);
        Task UpdateCartItemQuantity(int cartItemId, int quantity);
        Task<int> GetCartItemsQuantity();
    }
}
