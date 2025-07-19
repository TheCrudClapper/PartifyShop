using CSOS.Core.DTO.AccountDto;
using CSOS.Core.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace CSOS.Core.ServiceContracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">Registration data.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating success or failure.</returns>
        Task<IdentityResult> Register(RegisterRequest? request);

        /// <summary>
        /// Authenticates the user with the provided credentials.
        /// </summary>
        /// <param name="request">Login credentials.</param>
        /// <returns>A <see cref="SignInResult"/> indicating the result of the login attempt.</returns>
        Task<SignInResult> Login(LoginRequest? request);

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        Task Logout();

        /// <summary>
        /// Retrieves account data of the currently logged-in user for editing purposes.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing <see cref="AccountDto"/> with user data.</returns>
        Task<Result<AccountResponse>> GetAccountForEdit();

        /// <summary>
        /// Updates the currently logged-in user's account details.
        /// </summary>
        /// <param name="request">Updated account information.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the update.</returns>
        Task<Result> Edit(AccountUpdateRequest? request);

        /// <summary>
        /// Changes the password of the currently logged-in user.
        /// </summary>
        /// <param name="request">Password change data including current and new passwords.</param>
        /// <returns>A <see cref="Result"/> indicating the outcome of the operation.</returns>
        Task<Result> ChangePassword(PasswordChangeRequest? request);

        /// <summary>
        /// Retrieves detailed account information of the currently logged-in user.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing <see cref="AccountDetailsResponse"/> if successful.</returns>
        Task<Result<AccountDetailsResponse>> GetAccountDetailsAsync();
    }
}
