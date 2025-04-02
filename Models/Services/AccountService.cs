using ComputerServiceOnlineShop.Models.Abstractions;
using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _databaseContext;
        public AccountService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task Register(RegisterViewModel model)
        {
            string hashedPassword = HashPassword(model.Password);
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
            await _databaseContext.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
        }
        public async Task<User?> GetUser(LoginViewModel model)
        {
            return await _databaseContext.Users.SingleOrDefaultAsync(item => item.Email == model.Email);
        }

        public async Task<bool> IsUserInDatabase(RegisterViewModel model)
        {
            return await _databaseContext.Users.AnyAsync(item => item.Email == model.Email);
        }
        /// <summary>
        /// Method hashes user's password before adding him to database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
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
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }

        
    }
}
