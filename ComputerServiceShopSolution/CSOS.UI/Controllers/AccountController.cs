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
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ICountriesGetterService countriesGetterService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            _logger.LogInformation("GET Register Method called at time {Time}", DateTime.UtcNow); 
            
            var response = await _countriesGetterService.GetCountriesSelectionList();
            RegisterViewModel viewModel = new RegisterViewModel();
            viewModel.CountriesSelectionList = response.ToSelectListItem();
            return View(viewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            _logger.LogInformation("POST Register Method called at time {Time}", DateTime.UtcNow);
            
            var response = await _countriesGetterService.GetCountriesSelectionList();
            viewModel.CountriesSelectionList = response.ToSelectListItem();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during registration for {Email}", viewModel.Email);
                return View(viewModel);
            }

            RegisterRequest request = viewModel.ToRegisterRequest();
            IdentityResult result = await _accountService.Register(request);
            
            if (!result.Succeeded)
            {
                _logger.LogError("Registration failed for {Email}.", viewModel.Email);
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
                
            }
            _logger.LogInformation("User {Email} registered successfully", viewModel.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogInformation("GET Login Method called at time {Time}", DateTime.UtcNow);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request, string? returnUrl)
        {
            _logger.LogInformation("POST Login Method called at time {Time}", DateTime.UtcNow);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during logging in for User {Email}", request.Email);
                return View(request);
            }

            var result = await _accountService.Login(request);
            if (!result.Succeeded)
            {
                _logger.LogError("Login Failed for {Email}.", request.Email);
                ModelState.AddModelError("", "Invalid email or password");
                return View(request);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                _logger.LogInformation("User {Email} logged in successfully. Redirection to {Url}",  request.Email, returnUrl);
                return LocalRedirect(returnUrl);
            }
                
            
            _logger.LogInformation("User {Email} logged in successfully.",  request.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            _logger.LogInformation("User logged out at time {Time}", DateTime.UtcNow);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetails()
        {
            _logger.LogInformation("GET AccountDetails Method called at {Time}",  DateTime.UtcNow);
            var result = await _accountService.GetAccountDetailsAsync();

            if (result.IsFailure)
            {
                _logger.LogError("Failed to fetch Account Details for {UserName}", User.Identity!.Name);
                return View("Error", result.Error.Description);
            }
            
            var viewModel = new AccountDetailsViewModel
            {
                EditAddress = result.Value.AddressResponse.ToEditAddressViewModel(),
                UserDetails = result.Value.AccountResponse.ToUserDetailsViewModel(),
            };

            viewModel.EditAddress.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();

            _logger.LogInformation("Returning AccountDetails View");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Model State for {UserName}",  User.Identity!.Name);
                return PartialView("AccountPartials/_UserDetailsForm", viewModel);
            }
            
            AccountUpdateRequest request = viewModel.ToAccountUpdateRequest();
            var result = await _accountService.Edit(request);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to update user details for {UserName}", User.Identity!.Name);
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });
            }
                
            _logger.LogInformation("User details for {UserName} updated successfully",  User.Identity!.Name);
            return Json(new JsonResponseModel() { Success = true, Message = "User details updated successfully !" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest request)
        {
            _logger.LogInformation("POST ChangePassword Method called at {Time}",  DateTime.UtcNow);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Model State for PasswordChangeRequest");
                return PartialView("AccountPartials/_PasswordChangeForm", request);
            }
            
            var result = await _accountService.ChangePassword(request);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to change password for {UserName}", User.Identity!.Name);
                return Json(new JsonResponseModel() { Success = false, Message = $"Error: {result.Error.Description}" });
            }
            
            _logger.LogInformation("Successfully changed password for {UserName}", User.Identity!.Name);
            return Json(new JsonResponseModel() { Success = true, Message = "Password changed successfully !" });
        }
    }
}
