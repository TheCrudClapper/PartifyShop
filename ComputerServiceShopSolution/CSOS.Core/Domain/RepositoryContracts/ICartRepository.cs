using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface ICartRepository
    {
        /// <summary>
        /// Adds new object of CartItem to db
        /// </summary>
        /// <param name="cartItem">cart item it that will be added to db</param>
        /// <returns></returns>
        Task AddAsync(CartItem cartItem);

        /// <summary>
        /// Gets from db cart item of given cart id and offer id
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="offerId"></param>
        /// <returns>Returns CartItem Domain Model</returns>
        Task<CartItem?> GetCartItemAsync(int cartId, int offerId);

        /// <summary>
        /// Gets cart item from db based on its ID
        /// </summary>
        /// <param name="cartItemId">ID of cart item to get from db</param>
        /// <returns>Returns CartItem Domain Model</returns>
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);

        /// <summary>
        /// Gets cart by given ID from db
        /// </summary>
        /// <param name="cartId">ID of cart to get from db</param>
        /// <returns>Returns Cart Domanin Model</returns>
        Task<Cart?> GetCartByIdAsync(int cartId);

        /// <summary>
        /// Gets quantity of cart items in cart specified by cartId
        /// </summary>
        /// <param name="cartId">ID of cart</param>
        /// <returns></returns>
        Task<int> GetCartItemsQuantityAsync(int cartId);

        /// <summary>
        /// Gets an id of user with given userId
        /// </summary>
        /// <param name="userId">ID of user to get cart from</param>
        /// <returns></returns>
        Task<int?> GetLoggedUserCartIdAsync(Guid userId);

        /// <summary>
        /// Gets Cart Item with eager loaded Offer based on given cartItemId
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <returns>Return CartItem Domain Model</returns>
        Task<CartItem?> GetCartItemWithOfferAsync(int cartItemId);

        /// <summary>
        /// Gets cart with all possible eager loaded navigation properties
        /// </summary>
        /// <param name="cartId">Id of cart to be fetched</param>
        /// <returns>Returns CartItem Domain Model</returns>
        Task<Cart?> GetCartWithAllDetailsAsync(int cartId);

        /// <summary>
        /// Gets an IEnumerable collection of cart items based
        /// on cart ID with all necessary details
        /// </summary>
        /// <param name="cartId">ID of cart to get items from</param>
        /// <returns>Returns an IEnumerable of CartItems Domain Models</returns>
        Task<IEnumerable<CartItem>?> GetCartItemsForCostsUpdateAsync(int cartId);
    }
}
