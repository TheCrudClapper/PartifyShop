using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountriesService _countriesService;
        public AccountController(IAccountService accountService, ICountriesService countriesService)
        {
            _accountService = accountService;
            _countriesService = countriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel ViewModel = new RegisterViewModel()
            {
                CountriesSelectionList = await _countriesService.GetCountriesSelectionList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel ViewModel)
        {
            //initializing list of countries for view
            ViewModel.CountriesSelectionList = await _countriesService.GetCountriesSelectionList();

            if (!ModelState.IsValid)
            {
                return View(ViewModel);
            }

            IdentityResult result =  await _accountService.Register(ViewModel);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(ViewModel);
            }
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.Login(model);
            if (result.Succeeded)
            {
                if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid email or password");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
