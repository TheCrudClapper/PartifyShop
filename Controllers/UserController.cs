using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;
        private UserService _UserService { get; set; }
        public UserController(DatabaseContext context)
        {
            _context = context;
            _UserService = new UserService(_context);
        }
        public IActionResult GetUsers()
        {
            return View(_UserService.GetAllUsers());
        }
    }
}
