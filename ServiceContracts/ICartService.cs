using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ViewModels.CartViewModels;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface ICartService
    {
        Task AddToCart(int id);
        Task DeleteFromCart(int id);
        Task<bool> IsOfferInCart(int id);
        Task<CartViewModel> GetLoggedUserCart();
        Task<int> GetLoggedUserCartId();
        Task UpdateTotalCartValue(int id);
    }
}
