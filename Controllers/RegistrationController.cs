using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
