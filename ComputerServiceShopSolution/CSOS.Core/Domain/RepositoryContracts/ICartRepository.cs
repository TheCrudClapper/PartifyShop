using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface ICartRepository
    {
        Task AddAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemAsync(int cartId, int offerId);
        Task<CartItem?> GetCartItemAsync(int cartItemId);

        Task<Cart?> GetCartByIdAsync(int cartId);

        Task<int> GetCartItemsQuantityAsync(int cartId);

        Task<int?> GetLoggedUserCartIdAsync(Guid userId);

        Task<CartItem?> GetCartItemWithOfferAsync(int cartItemId);

        Task<Cart?> GetCartWithAllDetailsAsync(int cartId);

        Task<IEnumerable<CartItem>?> GetCartItemsForCostsUpdate(int cartId);
    }
}
