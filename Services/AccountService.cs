using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public async Task<IdentityResult> Register(RegisterDto dto)
        {
            var address = new Address()
            {
                Place = dto.Place,
                Street = dto.Street,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                HouseNumber = dto.HouseNumber,
                CountryId = dto.SelectedCountry,
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
                UserName = dto.Email,
                Address = address,
                FirstName = dto.FirstName,
                Title = dto.Title,
                Surname = dto.Surname,
                PhoneNumber = dto.PhoneNumber,
                Cart = cart,
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = dto.Email,
                NIP = dto.NIP,
            };
            var result =  await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }
        public async Task<SignInResult> Login(LoginDto dto)
        {
            var response = await _databaseContext.Users
                .AnyAsync(item => item.UserName == dto.Email && item.IsActive);

            if (!response)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure:false);
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
