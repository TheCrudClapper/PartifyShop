using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var result = await _cartService.GetLoggedUserCart();

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            var cart = result.Value.ToViewModel();
            return View(cart);
        }


        [HttpPost]
        //default quantity always 1
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var result = await _cartService.AddToCart(id, quantity);

            if (result.IsFailure)
                return Json(new { success = false, message = $"Error: {result.Error.Description}" });

            return Json(new { success = true, message = "Item added to cart successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            var result = await _cartService.DeleteFromCart(id);

            if (result.IsFailure)
                return Json(new { success = false, message = $"Error: {result.Error.Description}" });

            return Json(new { success = true, message = "Item removed from cart successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantityInCart(int cartItemId, int quantity)
        {
            var result = await _cartService.UpdateCartItemQuantity(cartItemId, quantity);

            if (result.IsFailure)
                return Json(new { success = false, message = $"Error: {result.Error.Description}" });

            return Json(new { success = true, message = "Updated cart successfully!" });
        }

        [HttpGet]
        public IActionResult GetCartItemsCount()
        {
            return ViewComponent("NavbarCart");
        }
    }
}