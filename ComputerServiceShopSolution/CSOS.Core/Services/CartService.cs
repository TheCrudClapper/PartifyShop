using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.DTO.Responses.CartItem;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Services
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IAccountService _accountService;
        public CartService(DatabaseContext databaseContext, IAccountService accountService)
        {
            _databaseContext = databaseContext;
            _accountService = accountService;
        }
        public async Task AddToCart(int offerId, int quantity = 1)
        {
            if(quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater that zero !");

            //downloading offer
            var offer = await _databaseContext.Offers
                .Where(item => item.Id == offerId && item.IsActive)
                .FirstOrDefaultAsync();

            if (offer == null)
                throw new InvalidOperationException("Couldn't find offer of given id");

            int cartId = await GetLoggedUserCartId();

            var existingItem = await _databaseContext.CartItems
                .Where(item => item.CartId == cartId && item.OfferId == offer.Id && item.IsActive)
                .FirstOrDefaultAsync();
            
            if(existingItem != null)
            {
                if(existingItem.Quantity + quantity <= offer.StockQuantity) {
                    existingItem.Quantity += quantity;
                    existingItem.DateCreated = DateTime.Now;
                }
                else
                    throw new InvalidOperationException("Cannot add more than is in shop");
            }
            else
            {
                if(quantity > offer.StockQuantity)
                    throw new InvalidOperationException("Invalid quantity. Please try again.");

                CartItem cartItem = new CartItem()
                {
                    CartId = cartId,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Offer = offer,
                    Quantity = quantity,
                }; 
                await _databaseContext.CartItems.AddAsync(cartItem);
            }
            await _databaseContext.SaveChangesAsync();
            await UpdateTotalCartValue(cartId);
        }

        public async Task DeleteFromCart(int cartItemId)
        {
            var cartItem = await _databaseContext.CartItems.Where(item => item.Id == cartItemId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
                throw new InvalidOperationException("Couldn't find such item in cart");

            var cartId = cartItem.CartId;

            //Soft Delete
            cartItem.IsActive = false;
            cartItem.DateDeleted = DateTime.Now;

            await _databaseContext.SaveChangesAsync();
            await UpdateTotalCartValue(cartId);
        }

        public async Task<CartResponseDto> GetLoggedUserCart()
        {
            var cartId = await GetLoggedUserCartId();

            return await _databaseContext.Carts
                .Where(item => item.Id == cartId)
                .Include(item => item.CartItems)
                    .ThenInclude(item => item.Offer)
                        .ThenInclude(item => item.Product)
                            .ThenInclude(item => item.ProductImages)
                .Select(item => new CartResponseDto
                {
                    CartItems = item.CartItems.Where(item => item.IsActive)
                    .Select(item => new CartItemResponseDto
                    {
                        Category = item.Offer.Product.ProductCategory.Name,
                        Condition = item.Offer.Product.Condition.ConditionTitle,
                        DateAdded = item.DateCreated,
                        Id = item.Id,
                        Price = item.Offer.Price,
                        Quantity = item.Quantity,
                        Title = item.Offer.Product.ProductName,
                        ImageUrl = item.Offer.Product.ProductImages.FirstOrDefault().ImagePath,
                        OfferId = item.OfferId,

                    }).ToList(),
                    TotalCartValue = item.TotalCartValue ?? 0,
                    TotalDeliveryValue = item.MinimalDeliveryValue ?? 0,
                    TotalItemsValue = item.TotalItemsValue ?? 0,

                }).FirstAsync();
        }

        public async Task<int> GetLoggedUserCartId()
        {
            Guid userId = _accountService.GetLoggedUserId();

            return await _databaseContext.Users
                .Where(user => user.IsActive && user.Id == userId && user.Cart.IsActive)
                .Select(item => item.Cart.Id)
                .FirstAsync();
        }

        public async Task UpdateTotalCartValue(int cartId)
        {
            var query = _databaseContext.CartItems
                .Where(cartItem => cartItem.CartId == cartId && cartItem.IsActive)
                .Include(offer => offer.Offer)
                    .ThenInclude(item => item.OfferDeliveryTypes)
                        .ThenInclude(item => item.DeliveryType)
                .AsQueryable();

            var totalValue = await query.SumAsync(item => item.Quantity * item.Offer.Price);

            var cartItems = await query.ToListAsync();

            var minimalDeliveryValue = cartItems.Select(item => item.Offer.OfferDeliveryTypes
                    .Select(item => item.DeliveryType.Price)
                    .DefaultIfEmpty(0)
                    .Min())
                .Sum();
            
            var cart = await _databaseContext.Carts.FirstOrDefaultAsync(item => item.Id == cartId && item.IsActive);
            if (cart != null)
            {
                cart.TotalItemsValue = totalValue;
                cart.TotalCartValue = totalValue + minimalDeliveryValue;
                cart.MinimalDeliveryValue = minimalDeliveryValue;
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var existingItem = await _databaseContext.CartItems
                .Include(item => item.Offer)
                .FirstOrDefaultAsync(item => item.Id == cartItemId && item.IsActive);

            if (existingItem?.Offer == null)
                throw new InvalidOperationException("Something went wrong");

            if (existingItem == null)
                throw new InvalidOperationException("Couldn't find this item in db");

            if (quantity <= 0)
            {
                await DeleteFromCart(cartItemId);
                return;
            }

            if (quantity <= existingItem.Offer.StockQuantity)
            {
                existingItem.Quantity = quantity;
                existingItem.DateEdited = DateTime.Now;
            }
            else
                existingItem.Quantity = existingItem.Offer.StockQuantity;

            await _databaseContext.SaveChangesAsync();
            await UpdateTotalCartValue(existingItem.CartId);
        }

        public async Task<int> GetCartItemsQuantity()
        {
            var cartId = await GetLoggedUserCartId();
            return await _databaseContext.CartItems.Where(item => item.CartId == cartId && item.IsActive)
                .SumAsync(item => item.Quantity);
        }
    }
}
