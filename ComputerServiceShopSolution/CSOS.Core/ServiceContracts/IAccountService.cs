using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;
namespace ComputerServiceOnlineShop.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers, adds new user to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IdentityResult> Register(RegisterDto dto);

        /// <summary>
        /// Login user into database 
        /// </summary>
        /// <param name="model">dto or viewmodel</param>
        /// <returns></returns>
        Task<SignInResult> Login(LoginDto dto);

        /// <summary>
        /// Allows user to logout
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// Returns Account Response Dto wiht details needed for edition
        /// </summary>
        /// <returns>Returns data of currently logged in user</returns>
        Task<Result<AccountDto>> GetAccountForEdit();

        /// <summary>
        /// Edits curently logged user using values from AccountDto
        /// </summary>
        /// <param name="dto">Object with updated data</param>
        /// <returns>Returns an result object identyfying success or failure</returns>
        Task<Result> Edit(AccountDto dto);

        /// <summary>
        /// Changes user's password 
        /// </summary>
        /// <param name="dto">DTO contains current, new, confirmed passwords to change</param>
        /// <returns>Returns an result object identyfying success or failure</returns>
        Task<Result> ChangePassword(PasswordDto dto);

        /// <summary>
        /// Gets all data about user for editing 
        /// </summary>
        /// <returns>Retruns result object representing succes or failure of opertion</returns>
        Task<Result<AccountDetailsDto>> GetAccountDetailsAsync();
    }
}
