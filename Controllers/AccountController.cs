using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.Models.Contexts;
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
        private readonly DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            //var countries = _context.Countries.Select(item => new SelectListItem()
            //{
            //    Value = item.Id.ToString(),
            //    Text = item.CountryName
            //}).ToList();
            //var viewModel = new RegisterViewModel
            //{
            //    Countries = countries,
            //};
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (IsUserLogged())
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _context.Users.AnyAsync(item => item.Email == model.Email))
            {
                return View(model);
            }

            string hashedPassword = HashPassword(model.Password);

            var address = new Address()
            {
                Place = model.Place,
                Street = model.Street,
                PostalCity = model.PostalCity,
                PostalCode = model.PostalCode,
                HouseNumber = model.HouseNumber,
                CountryId = model.CountryId,
                IsActive = true,
                DateCreated = DateTime.Now,
            };
            await _context.Addresses.AddAsync(address);
            var cart = new Cart()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
            };
            await _context.Carts.AddAsync(cart);
            var user = new User()
            {
                Address = address,
                FirstName = model.FirstName,
                Title = model.Title,
                Surname = model.Surname,
                PhoneNumber = model.PhoneNumber,
                Cart = cart,
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = model.Email,
                NIP = model.NIP,
                PasswordHash = hashedPassword,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        //for displaying login 
        public IActionResult Login()
        {
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

            var user = await _context.Users.SingleOrDefaultAsync(item => item.Email == model.Email);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Method verifies password from database to password entered by user
        /// </summary>
        /// <param name="inputPassword">Password provided by user</param>
        /// <param name="hashedPassword">Password taken from database</param>
        /// <returns></returns>
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }
    }
}
