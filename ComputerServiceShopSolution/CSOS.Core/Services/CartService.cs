using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Exceptions;
using CSOS.Core.Mappings.ToDto;

namespace ComputerServiceOnlineShop.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IOfferRepository _offerRepo;
        public CartService(IAccountService accountService, IOfferRepository offerRepository, ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepo = cartRepository;
            _offerRepo = offerRepository;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }
        //later delete taht and provide mocks
        public CartService() { }
        public async Task<Result> AddToCart(int offerId, int quantity = 1)
        {
            if (quantity <= 0)
                return Result.Failure(CartItemErrors.QuantityLowerThanZero);

            var offer = await _offerRepo.GetOfferByIdAsync(offerId);

            if (offer == null)
                return Result.Failure(CartItemErrors.CartItemDoesNotExists);

            int cartId = await GetLoggedUserCartId();

            var existingCartItem = await _cartRepo.GetCartItemAsync(cartId, offer.Id);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + quantity <= offer.StockQuantity)
                {
                    existingCartItem.Quantity += quantity;
                    existingCartItem.DateCreated = DateTime.Now;
                }
                else
                    return Result.Failure(CartItemErrors.CannotAddMoreToCart);
            }
            else
            {
                if (quantity > offer.StockQuantity)
                    return Result.Failure(CartItemErrors.InvalidItemQuantity);

                CartItem cartItem = new CartItem()
                {
                    CartId = cartId,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Offer = offer,
                    Quantity = quantity,
                };

                await _cartRepo.AddAsync(cartItem);
            }
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCartValue(cartId);

            return Result.Success();
        }

        public async Task<Result> DeleteFromCart(int cartItemId)
        {
            var cartItem = await _cartRepo.GetCartItemAsync(cartItemId);

            if (cartItem == null)
                return Result.Failure(CartItemErrors.CartItemDoesNotExists);

            var cartId = cartItem.CartId;

            //Soft Delete
            cartItem.IsActive = false;
            cartItem.DateDeleted = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCartValue(cartId);

            return Result.Success();
        }

        public async Task<Result<CartResponseDto>> GetLoggedUserCart()
        {
            var cartId = await GetLoggedUserCartId();

            var cart = await _cartRepo.GetCartWithAllDetailsAsync(cartId);

            if (cart == null)
                return Result.Failure<CartResponseDto>(CartErrors.CartDoesNotExist(cartId));

            var dto = cart.ToCartResponseDto();

            return dto;
        }

        public async Task<int> GetLoggedUserCartId()
        {
            Guid userId = _accountService.GetLoggedUserId();

            var cartId = await _cartRepo.GetLoggedUserCartIdAsync(userId);
            if (cartId == null)
                throw new EntityNotFoundException("User don't have a cart");

            return cartId.Value;
        }

        public async Task UpdateTotalCartValue(int cartId)
        {
            var cartItems = await _cartRepo.GetCartItemsForCostsUpdate(cartId);

            if (cartItems == null || cartItems.Count() == 0)
                return;

            var totalValue = CalculateItemsTotal(cartItems);

            var minimalDeliveryValue = CalculateMinimalDeliveryCost(cartItems);

            var cart = await _cartRepo.GetCartByIdAsync(cartId);

            if (cart == null)
                throw new EntityNotFoundException("Couldnt find user cart");

            cart.TotalItemsValue = totalValue;
            cart.TotalCartValue = totalValue + minimalDeliveryValue;
            cart.MinimalDeliveryValue = minimalDeliveryValue;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Result> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var existingItem = await _cartRepo.GetCartItemWithOfferAsync(cartItemId);

            if (existingItem == null || existingItem.Offer == null)
                return Result.Failure(CartItemErrors.CartItemDoesNotExists);

            if (quantity <= 0)
            {
                await DeleteFromCart(cartItemId);
                await UpdateTotalCartValue(existingItem.CartId);
                return Result.Success();
            }

            if (quantity <= existingItem.Offer.StockQuantity)
            {
                existingItem.Quantity = quantity;
                existingItem.DateEdited = DateTime.Now;
            }
            else
            {
                existingItem.Quantity = existingItem.Offer.StockQuantity;
                return Result.Failure(CartItemErrors.CannotAddMoreToCart);
            }
                
            await _unitOfWork.SaveChangesAsync();
            await UpdateTotalCartValue(existingItem.CartId);

            return Result.Success();
        }

        public async Task<int> GetCartItemsQuantity()
        {
            var cartId = await GetLoggedUserCartId();
            return await _cartRepo.GetCartItemsQuantityAsync(cartId);
        }

        public decimal CalculateItemsTotal(IEnumerable<CartItem> cartItems)
        {
            if(cartItems == null || cartItems.Count() == 0)
                return 0;

            return cartItems.Sum(item => item.Quantity * item.Offer.Price);
        }

        public decimal CalculateMinimalDeliveryCost(IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null || cartItems.Count() == 0)
                return 0;

            return cartItems.Select(item => item.Offer.OfferDeliveryTypes
                    .Select(item => item.DeliveryType.Price)
                    .DefaultIfEmpty(0)
                    .Min())
                .Sum();
        }
    }
}
