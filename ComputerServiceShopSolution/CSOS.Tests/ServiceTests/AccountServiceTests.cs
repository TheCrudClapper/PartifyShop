using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.Models.Services;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.ErrorHandling;
using CSOS.Core.ServiceContracts;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using AutoFixture;
namespace CSOS.Tests.ServiceTests
{
    public class AccountServiceTests
    {
        private readonly AccountService _accountService;

        private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IAccountRepository> _accountRepoMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IAddressService> _addressServiceMock = new();
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock = new();
        private readonly IFixture _fixture;

        public AccountServiceTests()
        {
            var userManager = new UserManager<ApplicationUser>(
                _userStoreMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>());

            var signInManager = new SignInManager<ApplicationUser>(
                userManager,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
                Mock.Of<IAuthenticationSchemeProvider>());

            _fixture = new Fixture();

            _accountService = new AccountService(
                userManager,
                signInManager,
                _currentUserServiceMock.Object,
                _accountRepoMock.Object,
                _unitOfWorkMock.Object,
                _addressServiceMock.Object,
                _countriesGetterServiceMock.Object);
        }


        #region GetAccountForEdit Method Tests
        [Fact]
        public async Task GetAccountForEdit_UserResultFailure_ReturnFailureResult()
        {
            //Arrange
            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(Result.Failure<ApplicationUser>(AccountErrors.AccountNotFound));

            //Act
            var result = await _accountService.GetAccountForEdit();

            //Assert
            result.Should().BeOfType<Result<AccountDto>>();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountErrors.AccountNotFound);

        }

        [Fact]
        public async Task GetAccountForEdit_ValidUserId_ReturnsDto()
        {
            //Arrange
            ApplicationUser applicationUser = _fixture.Build<ApplicationUser>()
                .Without(item => item.Address)
                .Without(item => item.Offers)
                .Without(item => item.Cart)
                .Create();

            _currentUserServiceMock.Setup(item => item.GetCurrentUserAsync()).ReturnsAsync(applicationUser);

            //Act
            var result = await _accountService.GetAccountForEdit();

            //Assert
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<AccountDto>();
            result.IsSuccess.Should().BeTrue();
            result.Value.PhoneNumber.Should().Be(applicationUser.PhoneNumber);
            result.Value.NIP.Should().Be(applicationUser.NIP);
            result.Value.Surname.Should().Be(applicationUser.Surname);
            result.Value.FirstName.Should().Be(applicationUser.FirstName);
        }
        #endregion

    }
}
