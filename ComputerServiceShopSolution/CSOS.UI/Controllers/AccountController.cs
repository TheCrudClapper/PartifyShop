using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.Universal;
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
            var response = await _countriesService.GetCountriesSelectionList();
            RegisterViewModel ViewModel = new RegisterViewModel()
            {
                CountriesSelectionList = response.ConvertToSelectListItem()
            };
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            //initializing list of countries for view
            var response  = await _countriesService.GetCountriesSelectionList();
            viewModel.CountriesSelectionList = response.ConvertToSelectListItem();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            RegisterDto dto = viewModel.ToRegisterDto();
            IdentityResult result = await _accountService.Register(dto);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            LoginDto dto = viewModel.ToLoginDto();
            var result = await _accountService.Login(dto);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid email or password");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetails()
        {
            return View();
        }
    }
}
