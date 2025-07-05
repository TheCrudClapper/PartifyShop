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
        private readonly ICountriesGetterService _countriesGetterService;
        
        public AccountService
            (UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICurrentUserService currentUserService,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IAddressService addressService,
            ICountriesGetterService countriesGetterService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _accountRepo = accountRepository;
            _signInManager = signInManager;
            _addressService = addressService;
            _countriesGetterService = countriesGetterService;
            _currentUserService = currentUserService;
        }


        public async Task<IdentityResult> Register(RegisterDto dto)
        {
            Address address = dto.ToAddressEntity();

            Cart cart = new Cart() { IsActive = true, DateCreated = DateTime.UtcNow };

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
            var userResult = await _currentUserService.GetCurrentUserAsync();

            if (userResult.IsFailure)
                return Result.Failure<AccountDto>(AccountErrors.AccountNotFound);

            var dto = userResult.Value.ToAccountResponseDto();

            return dto;
        }

        public async Task<Result> Edit(AccountDto dto)
        {
            var userResult = await _currentUserService.GetCurrentUserAsync();

            if (userResult.IsFailure)
                return Result.Failure(userResult.Error);

            var user = userResult.Value;

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
            var userResult = await _currentUserService.GetCurrentUserAsync();
            if(userResult.IsFailure)
                return Result.Failure(AccountErrors.AccountNotFound);

            var user = userResult.Value;

            if (!await _userManager.CheckPasswordAsync(user, dto.CurrentPassword))
                return Result.Failure(AccountErrors.WrongPassword);

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

            var countries = await _countriesGetterService.GetCountriesSelectionList();

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
