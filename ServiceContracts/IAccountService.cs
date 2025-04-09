using ComputerServiceOnlineShop.Models.IdentityEntities;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ComputerServiceOnlineShop.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers, adds new user to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IdentityResult> Register(RegisterViewModel model);
        /// <summary>
        /// Login user into database 
        /// </summary>
        /// <param name="model">dto or viewmodel</param>
        /// <returns></returns>
        Task<SignInResult> Login(LoginViewModel model);
        /// <summary>
        /// Allows user to logout
        /// </summary>
        /// <returns></returns>
        Task Logout();
        /// <summary>
        /// Method serches for currently logged user
        /// </summary>
        /// <returns>Returns GUID of currently logged in user</returns>
        Guid GetLoggedUserId();
    }
}
