using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
