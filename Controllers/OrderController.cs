using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class OrderController : Controller
    {
        public async Task<IActionResult> AddOrder()
        {
            return View();
        }
    }
}
