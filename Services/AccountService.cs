using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.Models.IdentityEntities;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(DatabaseContext databaseContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            var address = new Address()
            {
                Place = model.Place,
                Street = model.Street,
                PostalCity = model.PostalCity,
                PostalCode = model.PostalCode,
                HouseNumber = model.HouseNumber,
                CountryId = int.Parse(model.SelectedCountry),
                IsActive = true,
                DateCreated = DateTime.Now,
            };
            await _databaseContext.Addresses.AddAsync(address);

            var cart = new Cart()
            {
                IsActive = true,
                DateCreated = DateTime.Now,
            };
            await _databaseContext.Carts.AddAsync(cart);
            await _databaseContext.SaveChangesAsync();

            var user = new ApplicationUser()
            {
                UserName = model.Email,
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
            };
            var result =  await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }
        public async Task<SignInResult> Login(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure:false);
        }

        public async Task Logout()
        {
             await _signInManager.SignOutAsync();
        }

        public Guid GetLoggedUserId()
        {
            var userIdString =  _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User is not authenticated, or id is invalid");
        }
    }
}
