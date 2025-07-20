using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IConfigurationReader _configurationReader;

        public CartController(ICartService cartService, IConfigurationReader configurationReader)
        {
            _cartService = cartService;
            _configurationReader = configurationReader;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _cartService.GetLoggedUserCart();

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            var cart = result.Value.ToCartViewModel(_configurationReader);
            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var result = await _cartService.AddToCart(id, quantity);

            if (result.IsFailure)
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });

            return Json(new JsonResponseModel { Success = true, Message = "Item Successfully Added to Cart" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            var result = await _cartService.DeleteFromCart(id);

            if (result.IsFailure)
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });

            return Json(new JsonResponseModel { Success = true, Message = "Item removed from cart successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantityInCart(int cartItemId, int quantity)
        {
            var result = await _cartService.UpdateCartItemQuantity(cartItemId, quantity);

            if (result.IsFailure)
                return Json(new JsonResponseModel { Success = false, Message = $"Error: {result.Error.Description}" });

            return Json(new JsonResponseModel { Success = true, Message = "Updated cart successfully!" });
        }

        [HttpGet]
        public IActionResult GetCartItemsCount()
        {
            return ViewComponent("NavbarCart");
        }
    }
}