using ComputerServiceOnlineShop.ServiceContracts;
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
            //donwload data from db about cart
            var cart = await _cartService.GetLoggedUserCart();
            return View(cart);
        }

        
        [HttpPost]
        //default quantity always 1
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCart(id, quantity);
                TempData["SuccessMessage"] = "Item added to cart successfully !";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            try
            {
                await _cartService.DeleteFromCart(id);
                TempData["SuccessMessage"] = "Item removed from cart successfully";
            }
            catch(InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            
            return RedirectToAction("Cart");
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
