using Moq;
using AutoFixture;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using CSOS.Core.ErrorHandling;
using CSOS.UI.ViewModels.AccountViewModels;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.Core.DTO.Responses.Account;
using CSOS.Core.ServiceContracts;
using CSOS.UI;

namespace CSOS.Tests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly IFixture _fixture;
        private AccountController _accountController;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _addressServiceMock = new Mock<IAddressService>();
            _accountService = _accountServiceMock.Object;
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _addressService = _addressServiceMock.Object;
            _fixture = new Fixture();
        }

        private AccountController CreateController()
        {
            return new AccountController(_accountService, _countriesGetterService, _addressService);
        }

        #region Register GET Method Tests

        [Fact]
        public async Task Register_ReturnsViewWithViewModel()
        {
            //Arrange
            var countries = _fixture.CreateMany<SelectListItemDto>(3);
            _countriesGetterServiceMock.Setup(item => item.GetCountriesSelectionList()).ReturnsAsync(countries);
            var controller = CreateController();

            //Act
            IActionResult result = await controller.Register();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = viewResult.Model.Should().BeOfType<RegisterViewModel>().Subject;
            viewModel.CountriesSelectionList.Should().HaveCount(countries.Count());
        }

        #endregion

        #region Register POST Method Tests

        [Fact]
        public async Task Register_SuccededRegister_RedirectToIndex()
        {
            //Arrange
            RegisterViewModel viewModel = _fixture.Build<RegisterViewModel>()
                .With(item => item.SelectedCountry, "1")
                .Create();

            _accountServiceMock.Setup(item => item.Register(It.IsAny<RegisterDto>())).ReturnsAsync(IdentityResult.Success);

            _accountController = CreateController();

            //Act
            IActionResult result = await _accountController.Register(viewModel);

            //Assert
            RedirectToActionResult redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirect.ControllerName.Should().Be("Home");
            redirect.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Register_FailedRegister_ReturnView()
        {
            //Arrange
            RegisterViewModel viewModel = _fixture.Build<RegisterViewModel>()
                .With(item => item.SelectedCountry, "1")
                .Create();

            _accountServiceMock.Setup(item => item.Register(It.IsAny<RegisterDto>())).ReturnsAsync(IdentityResult.Failed());

            _accountController = CreateController();

            //Act
            IActionResult result = await _accountController.Register(viewModel);

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<RegisterViewModel>();
        }
        #endregion

        #region Login GET Method Tests
        [Fact]
        public void Login_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();

            //Act
            IActionResult result = _accountController.Login();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Login POST Method Tests
        [Fact]
        public async Task Login_InvalidModelState_ReturnsView()
        {
            //Arrange
            LoginViewModel viewModel = _fixture.Create<LoginViewModel>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("key1", "Test dummy error");

            //Act
            IActionResult result = await _accountController.Login(viewModel, It.IsAny<string>());

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async Task Login_SuccededLogin_RedirectsToAction()
        {
            //Arrange
            LoginViewModel viewModel = _fixture.Create<LoginViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            //Act
            IActionResult result = await _accountController.Login(viewModel, String.Empty);

            //Arrange
            RedirectToActionResult redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task Login_ReturnUrlValid_ReturnsLocalRedirect()
        {
            //Arrange
            LoginViewModel viewModel = _fixture.Create<LoginViewModel>();

            string localUrl = "~/Offer/OfferBrowser";

            _accountController = CreateController();

            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(item => item.IsLocalUrl(localUrl)).Returns(true);

            _accountController.Url = urlHelperMock.Object;

            //Act
            IActionResult result = await _accountController.Login(viewModel, localUrl);

            //Assert
            LocalRedirectResult localRedirectResult = result.Should().BeOfType<LocalRedirectResult>().Subject;
            localRedirectResult.Url.Should().Be(localUrl);
        }

        [Fact]
        public async Task Login_FailedLogin_ReturnsView()
        {
            //Arrange
            LoginViewModel viewModel = _fixture.Create<LoginViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Login(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Failed);

            //Act
            IActionResult result = await _accountController.Login(viewModel, String.Empty);

            //Arrange
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(viewModel);
            _accountController.ModelState.IsValid.Should().BeFalse();
            _accountController.ModelState["Login"]!.Errors.Should().NotBeNull();
        }
        #endregion

        #region Logout Method Tests
        [Fact]
        public async Task Logout_RedirectsToAction()
        {
            //Arrange
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Logout()).Returns(Task.CompletedTask);

            //Act
            IActionResult result = await _accountController.Logout();

            //Assert
            RedirectToActionResult redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.ControllerName.Should().Be("Home");
        }
        #endregion

        #region AccountDetails GET Method Tests
        [Fact]
        public async Task AccountDetails_FailureServiceResult_ReturnsErrorView()
        {
            //Arrange
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.GetAccountDetailsAsync()).ReturnsAsync(Result.Failure<AccountDetailsDto>(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _accountController.AccountDetails();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.ViewName.Should().Be("Error");
            var viewModel = viewResult.Model.Should().BeOfType<string>();
            viewModel.Subject.Should().BeSameAs(AddressErrors.AddressNotFound.Description);
        }


        //To be repiared and the whole edit addres mess fixed
        [Fact]
        public async Task AccountDetails_SuccessServiceResult_ReturnsView()
        {
            //Arrange
            _accountController = CreateController();
            AccountDetailsDto accountDetailsDto = _fixture.Create<AccountDetailsDto>();
           
            AccountDetailsViewModel expectedViewModel = new AccountDetailsViewModel()
            {
                EditAddress = accountDetailsDto.EditAddressResponseDto.ToViewModel(),
                UserDetails = accountDetailsDto.AccountDto.ToUserDetailsViewModel(),
            };
            
            IEnumerable<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>();

            expectedViewModel.EditAddress.CountriesSelectionList = countries.ToSelectListItem();
            accountDetailsDto.EditAddressResponseDto.CountriesSelectionList = countries.ToList();

            _accountServiceMock.Setup(item => item.GetAccountDetailsAsync()).ReturnsAsync(Result.Success(accountDetailsDto));

            //Act
            IActionResult result = await _accountController.AccountDetails();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var viewModel = viewResult.Model.Should().BeOfType<AccountDetailsViewModel>();
            //Assert wheret the second item is right
            viewModel.Subject.UserDetails.Should().BeEquivalentTo(expectedViewModel.UserDetails);
        }
        #endregion

        #region Edit POST Method Tests
        [Fact]
        public async Task Edit_InvalidModelState_ReturnsPartial()
        {
            //Arrange
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("test", "test");

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Arrange
            PartialViewResult partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async Task Edit_FailureServiceResult_ReturnsErrorJson()
        {
            //Arrange
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Edit(It.IsAny<AccountDto>())).ReturnsAsync(Result.Failure(AccountErrors.AccountNotFound));

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeFalse();
            responseModel.Message.Should().Be($"Error: {AccountErrors.AccountNotFound.Description}");
        }

        [Fact]
        public async Task Edit_SuccessServiceResult_ReturnsErrorJson()
        {
            //Arrange
            UserDetailsViewModel viewModel = _fixture.Create<UserDetailsViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.Edit(It.IsAny<AccountDto>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _accountController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeTrue();
            responseModel.Message.Should().Be("User details updated successfully !");
        }
        #endregion

        #region ChangePassword POST Method Tests

        [Fact]
        public async Task ChangePassword_InvalidModelState_ReturnsPartial()
        {
            //Arrange
            PasswordChangeViewModel viewModel = _fixture.Create<PasswordChangeViewModel>();
            _accountController = CreateController();
            _accountController.ModelState.AddModelError("test", "test");
            
            //Act
            IActionResult result = await  _accountController.ChangePassword(viewModel);
            
            //Assert
            PartialViewResult partialViewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            partialViewResult.Model.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async Task ChangePassword_FailureServiceResult_ReturnsErrorJson()
        {
            //Arrange
            PasswordChangeViewModel viewModel = _fixture.Create<PasswordChangeViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.ChangePassword(It.IsAny<PasswordChangeRequest>()))
                .ReturnsAsync(Result.Failure(AccountErrors.PasswordChangeFailed));
            
            //Act
            IActionResult result = await _accountController.ChangePassword(viewModel);
            
            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeFalse();
            responseModel.Message.Should().Be($"Error: {AccountErrors.PasswordChangeFailed.Description}");
        }

        [Fact]
        public async Task ChangePassword_SuccessServiceResult_ReturnsErrorJson()
        {
            //Arrange
            PasswordChangeViewModel viewModel = _fixture.Create<PasswordChangeViewModel>();
            _accountController = CreateController();
            _accountServiceMock.Setup(item => item.ChangePassword(It.IsAny<PasswordChangeRequest>()))
                .ReturnsAsync(Result.Success);
            
            //Act
            IActionResult result = await _accountController.ChangePassword(viewModel);
            
            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var responseModel = jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject;
            responseModel.Success.Should().BeTrue();
            responseModel.Message.Should().Be("Password changed successfully !");
        }
        #endregion
    }
}
