using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.Mappings.ToEntity.AddressMappings;
using CSOS.Core.Mappings.ToEntity.ApplicationUserMappings;
using CSOS.Core.Mappings.ToEntity.CartMappings;
using CSOS.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountRepository _accountRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressService _addressService;
        private readonly ICountriesService _countriesService;
        
        public AccountService
            (UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICurrentUserService currentUserService,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IAddressService addressService,
            ICountriesService countriesService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _accountRepo = accountRepository;
            _signInManager = signInManager;
            _addressService = addressService;
            _countriesService = countriesService;
            _currentUserService = currentUserService;
        }


        public async Task<IdentityResult> Register(RegisterDto dto)
        {
            Address address = dto.ToAddressEntity();

            Cart cart = dto.ToCartEntity();

            await _unitOfWork.SaveChangesAsync();

            ApplicationUser user = dto.ToApplicationUserEntity(address, cart);
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);

            return result;
        }

        public async Task<SignInResult> Login(LoginDto dto)
        {
            var response = await _accountRepo.IsUserByEmailInDatabaseAsync(dto.Email);

            if (!response)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: dto.isPersistent, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Result<AccountDto>> GetAccountForEdit()
        {
            var userId = _currentUserService.GetUserId();

            ApplicationUser? user = await _accountRepo.GetUserByIdAsync(userId);

            if (user == null)
                return Result.Failure<AccountDto>(AccountErrors.AccountNotFound);

            var dto = user.ToAccountResponseDto();

            return dto;
        }

        public async Task<Result> Edit(AccountDto dto)
        {
            var userId = _currentUserService.GetUserId();

            ApplicationUser? user = await _accountRepo.GetUserByIdAsync(userId);

            if (user == null)
                return Result.Failure(AccountErrors.AccountNotFound);

            //updating fields
            user.Title = dto.Title;
            user.PhoneNumber = dto.PhoneNumber;
            user.NIP = dto.NIP;
            user.FirstName = dto.FirstName;
            user.Surname = dto.Surname;
            user.DateEdited = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ChangePassword(PasswordDto dto)
        {
            var userId = _currentUserService.GetUserId();

            ApplicationUser? user = await _accountRepo.GetUserByIdAsync(userId);

            if (user == null)
                return Result.Failure(AccountErrors.AccountNotFound);

            if (!await _userManager.CheckPasswordAsync(user, dto.ConfirmPassword))
                return Result.Failure(AccountErrors.WrongPassword);

            if (dto.ConfirmPassword != dto.NewPassword)
                return Result.Failure(AccountErrors.PasswordsDoestMatch);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if(!result.Succeeded)
                return Result.Failure(AccountErrors.PasswordChangeFailed);

            return Result.Success();
        }

        public async Task<Result<AccountDetailsDto>> GetAccountDetailsAsync()
        {
            var addressResult = await _addressService.GetAddressForEdit();
            if (addressResult.IsFailure)
                return Result.Failure<AccountDetailsDto>(AddressErrors.AddressNotFound);

            var accountResult = await GetAccountForEdit();
            if (accountResult.IsFailure)
                return Result.Failure<AccountDetailsDto>(AccountErrors.AccountNotFound);

            var countries = await _countriesService.GetCountriesSelectionList();

            var addressDto = addressResult.Value;
            addressDto.CountriesSelectionList = countries;

            var accountDto = accountResult.Value;

            return new AccountDetailsDto()
            {
                EditAddressResponseDto = addressDto,
                AccountDto = accountDto,
            };
        }
    }
}
