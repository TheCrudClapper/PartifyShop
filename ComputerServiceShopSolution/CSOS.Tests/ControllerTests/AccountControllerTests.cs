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
            viewResult.Model.Should().BeOfType<RegisterViewModel>();

            var viewModel = Assert.IsType<RegisterViewModel>(viewResult.Model);
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
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
            redirect.ControllerName.Should().Be("Home");
            redirect.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Register_FailedRegister_ReturnViewWithViewModel()
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
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            var data = Assert.IsType<RegisterViewModel>(viewResult.Model);
        }
        #endregion
    }
}
