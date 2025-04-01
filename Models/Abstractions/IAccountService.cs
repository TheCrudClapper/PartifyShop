using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ComputerServiceOnlineShop.Models.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers, adds new user to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Register(RegisterViewModel model);
        /// <summary>
        /// Gets user from database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<User?> GetUser(LoginViewModel model);
        /// <summary>
        /// Hashed user inputted password using sha256 hasing method
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Returns User</returns>
        string HashPassword(string password);
        /// <summary>
        /// Method checks wheter hashes are the same
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>Returns true if hashes are identical</returns>
        bool VerifyPassword(string inputPassword, string hashedPassword);
        /// <summary>
        /// Method that seraches in database for user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returs boolean value on success</returns>
        Task<bool> IsUserInDatabase(RegisterViewModel model);
        /// <summary>
        /// Method gets countries for select
        /// </summary>
        /// <returns>Returns asynchronous list item</returns>
        Task<List<SelectListItem>> GetCountries();
    }
}
