using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace ComputerServiceOnlineShop.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="dto">Registration data.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success or failure.</returns>
        Task<IdentityResult> Register(RegisterDto dto);

        /// <summary>
        /// Authenticates the user with the provided credentials.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>A <see cref="SignInResult"/> indicating the result of the login attempt.</returns>
        Task<SignInResult> Login(LoginDto dto);

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        Task Logout();

        /// <summary>
        /// Retrieves account data of the currently logged-in user for editing purposes.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing <see cref="AccountDto"/> with user data.</returns>
        Task<Result<AccountDto>> GetAccountForEdit();

        /// <summary>
        /// Updates the currently logged-in user's account details.
        /// </summary>
        /// <param name="dto">Updated account information.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the update.</returns>
        Task<Result> Edit(AccountDto dto);

        /// <summary>
        /// Changes the password of the currently logged-in user.
        /// </summary>
        /// <param name="dto">Password change data including current and new passwords.</param>
        /// <returns>A <see cref="Result"/> indicating the outcome of the operation.</returns>
        Task<Result> ChangePassword(PasswordChangeRequest dto);

        /// <summary>
        /// Retrieves detailed account information of the currently logged-in user.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing <see cref="AccountDetailsDto"/> if successful.</returns>
        Task<Result<AccountDetailsDto>> GetAccountDetailsAsync();
    }
}
