using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AuctionController : Controller
    {
        public IActionResult AddAuction()
        {
            return View();
        }
        public IActionResult AllUserAuctions()
        {
            return View();
        }
        public IActionResult ShowAuction()
        {
            return View();
        }
        public IActionResult AuctionBrowser()
        {
            return View();
        }
    }
}
