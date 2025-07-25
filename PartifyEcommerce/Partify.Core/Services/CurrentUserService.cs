using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CSOS.Core.ResultTypes;

namespace CSOS.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountRepository _accountRepo;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
            IAccountRepository accountRepo)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _accountRepo = accountRepo;
        }

        public async Task<Result<ApplicationUser>> GetCurrentUserAsync()
        {
            var userId = GetUserId();
            var user = await _accountRepo.GetUserByIdAsync(userId);

            return user != null ? Result.Success(user) : Result.Failure<ApplicationUser>(AccountErrors.AccountNotFound);
        }

        public Guid GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("No HTTP context or unauthenticated user.");

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("NameIdentifier claim is missing.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID format.");

            return userId;
        }

    }
}
