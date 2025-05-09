using ComputerServiceOnlineShop.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAccountService _accountService;
        public AddressController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

    }
}
