using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Account;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly ICountriesGetterService _countriesGetterService;
        public AccountController(IAccountService accountService, ICountriesGetterService countriesGetterService, IAddressService addressService)
        {
            _accountService = accountService;
            _countriesGetterService = countriesGetterService;
            _addressService = addressService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var response = await _countriesGetterService.GetCountriesSelectionList();
            RegisterViewModel viewModel = new RegisterViewModel();
            viewModel.CountriesSelectionList = response.ConvertToSelectListItem();
            return View(viewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var response = await _countriesGetterService.GetCountriesSelectionList();
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
            var result = await _accountService.GetAccountDetailsAsync();
            if (result.IsFailure)
                return View("Error", result.Error.Description);

            var viewModel = new AccountDetailsViewModel
            {
                EditAddress = result.Value.EditAddressResponseDto.ToViewModel(),
                UserDetails = result.Value.AccountDto.ToUserDetailsViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return PartialView("AccountPartials/_UserDetailsForm", viewModel);

            AccountDto dto = viewModel.ToAccountDto();
            var result = await _accountService.Edit(dto);

            if (result.IsFailure)
            {
                //find a way to handle errors
                ModelState.AddModelError("Error", result.Error.Description);
                return PartialView("AccountPartials/_UserDetailsForm", viewModel);
            }
                
            return PartialView("AccountPartials/_UserDetailsForm", viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction();

            return RedirectToAction();
        }
    }
}
