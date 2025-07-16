using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using Moq;
using AutoFixture;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ViewModels.AccountViewModels;
using CSOS.Core.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CSOS.Tests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ICountriesGetterService>  _countriesGetterServiceMock;
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
            var data = Assert.IsType<RegisterViewModel>(viewResult.Model);
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
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
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
            
            var UrlHelperMock = new Mock<IUrlHelper>();
            UrlHelperMock.Setup(item => item.IsLocalUrl(localUrl)).Returns(true);

            _accountController.Url = UrlHelperMock.Object;

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
    }
}
