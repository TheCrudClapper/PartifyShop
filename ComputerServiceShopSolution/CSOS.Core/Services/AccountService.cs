using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.Mappings.ToEntity.AddressMappings;
using CSOS.Core.Mappings.ToEntity.ApplicationUserMappings;
using CSOS.Core.Mappings.ToEntity.CartMappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountRepository _accountRepo;
        public AccountService
            (UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _accountRepo = accountRepository;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IdentityResult> Register(RegisterDto dto)
        {
            Address address = dto.ToAddressEntity();

            Cart cart = dto.ToCartEntity();

            await _unitOfWork.SaveChangesAsync();

            ApplicationUser user = dto.ToApplicationUserEntity(address, cart);
            var result =  await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);

            return result;
        }
        public async Task<SignInResult> Login(LoginDto dto)
        {
            var response = await _accountRepo.IsUserByEmailInDatabaseAsync(dto.Email);

            if (!response)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: dto.isPersistent, lockoutOnFailure:false);
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
