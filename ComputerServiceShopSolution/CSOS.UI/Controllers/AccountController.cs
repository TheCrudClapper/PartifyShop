using CSOS.Core.DTO.AccountDto;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountriesGetterService _countriesGetterService;
        public AccountController(IAccountService accountService, ICountriesGetterService countriesGetterService)
        {
            _accountService = accountService;
            _countriesGetterService = countriesGetterService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var response = await _countriesGetterService.GetCountriesSelectionList();
            RegisterViewModel viewModel = new RegisterViewModel();
            viewModel.CountriesSelectionList = response.ToSelectListItem();
            return View(viewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var response = await _countriesGetterService.GetCountriesSelectionList();
            viewModel.CountriesSelectionList = response.ToSelectListItem();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            RegisterRequest request = viewModel.ToRegisterRequest();
            IdentityResult result = await _accountService.Register(request);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
                
            }
            return RedirectToAction("Index", "Home");
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

            LoginRequest request = viewModel.ToLoginRequest();
            var result = await _accountService.Login(request);
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
                EditAddress = result.Value.AddressResponse.ToEditAddressViewModel(),
                UserDetails = result.Value.AccountResponse.ToUserDetailsViewModel(),
            };

            viewModel.EditAddress.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return PartialView("AccountPartials/_UserDetailsForm", viewModel);

            AccountUpdateRequest request = viewModel.ToAccountUpdateRequest();
            var result = await _accountService.Edit(request);

            if (result.IsFailure)
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });

            return Json(new JsonResponseModel() { Success = true, Message = "User details updated successfully !" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return PartialView("AccountPartials/_PasswordChangeForm", viewModel);

            PasswordChangeRequest dto = viewModel.ToPasswordChangeRequest();
            var result = await _accountService.ChangePassword(dto);

            if(result.IsFailure)
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });

            return Json(new JsonResponseModel() { Success = true, Message = "Password changed successfully !" });

        }
    }
}
