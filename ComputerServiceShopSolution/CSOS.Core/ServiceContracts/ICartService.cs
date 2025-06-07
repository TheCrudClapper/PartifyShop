using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.ErrorHandling;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface ICartService
    {
        Task<Result> AddToCart(int offerId, int quantity = 1);
        Task<Result> DeleteFromCart(int id);
        Task<Result<CartResponseDto>> GetLoggedUserCart();
        Task<int> GetLoggedUserCartId();
        Task UpdateTotalCartValue(int id);
        Task UpdateCartItemQuantity(int cartItemId, int quantity);
        Task<int> GetCartItemsQuantity();
    }
}
