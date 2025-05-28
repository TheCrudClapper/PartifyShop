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
            var response = await _cartService.GetLoggedUserCart();
            var cart = response.ToViewModel();
            return View(cart);
        }

        
        [HttpPost]
        //default quantity always 1
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCart(id, quantity);
                return Json(new { success = true, message = "Item added to cart successfully!" });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Unexpected server error. Please try again later." });
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            try
            {
                await _cartService.DeleteFromCart(id);
                TempData["SuccessMessage"] = "Item removed from cart successfully";
                return Json(new { success = true, message = "Item removed from cart successfully!" });
            }
            catch(InvalidOperationException ex)
            { 
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Unexpected server error. Please try again later." });
            }
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
