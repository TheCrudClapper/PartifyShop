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

            //handle message not showing affter last item is deleted from cart. The page chagnes
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantityInCart(int cartItemId, int quantity)
        {
            try
            {
                await _cartService.UpdateCartItemQuantity(cartItemId, quantity);
                TempData["SuccessMessage"] = "Updated cart successfully !";
            }
            catch(InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("Cart");
        }
    }
}
