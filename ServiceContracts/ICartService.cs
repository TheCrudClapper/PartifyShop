using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ViewModels.CartViewModels;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface ICartService
    {
        Task AddToCart(int offerId, int quantity = 1);
        Task DeleteFromCart(int id);
        Task<CartViewModel> GetLoggedUserCart();
        Task<int> GetLoggedUserCartId();
        Task UpdateTotalCartValue(int id);
        Task UpdateCartItemQuantity(int cartItemId, int quantity);
    }
}
