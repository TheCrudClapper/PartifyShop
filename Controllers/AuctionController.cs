using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AuctionController : Controller
    {
        public IActionResult Auction()
        {
            return View();
        }
        public IActionResult Auctions()
        {
            return View();
        }
    }
}
