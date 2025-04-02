using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.Models.Abstractions;
using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.Models.Services;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountriesService _countriesService;
        public AccountController(IAccountService accountService, ICountriesService countriesService)
        {
            _accountService = accountService;
            _countriesService = countriesService;
        }
        public async Task<IActionResult> Register()
        {
            RegisterViewModel ViewModel = new RegisterViewModel()
            {
                CountriesSelectionList = await _countriesService.GetCountriesSelectionList()
            };
            if (IsUserLogged())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel ViewModel)
        {
            //initializing list of countries for view
            ViewModel.CountriesSelectionList = await _countriesService.GetCountriesSelectionList();

            if (IsUserLogged())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                if (await _accountService.IsUserInDatabase(ViewModel))
                {
                    ViewData["userExists"] = "User with that email already exists !";
                }
                return View(ViewModel);
            }

            await _accountService.Register(ViewModel);
            return RedirectToAction("Login");
        }
        //for displaying login 
        public IActionResult Login()
        {
            if (IsUserLogged())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (IsUserLogged())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.GetUser(model);
            if (user == null || !_accountService.VerifyPassword(model.Password, user.PasswordHash))
            {
                ViewData["loginError"] = "Invalid email or password.";
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Method to log out user by clearing the session
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Methods check wheter user is logged in
        /// </summary>
        /// <returns> Method returns true if user is logged, else false</returns>
        private bool IsUserLogged()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return true;
            return false;
        }
    }
}
