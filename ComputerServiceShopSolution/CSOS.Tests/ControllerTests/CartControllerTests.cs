using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.UI.Helpers.Contracts;
using Moq;
using AutoFixture;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.ErrorHandling;
using CSOS.UI.Mappings.ToViewModel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.Tests.ControllerTests
{
    public class CartControllerTests
    {
        private readonly ICartService _cartService;
        private readonly CartController _cartController;
        private readonly IConfigurationReader _configurationReader;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Mock<IConfigurationReader> _configurationReaderMock;
        private readonly IFixture _fixture;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _configurationReaderMock = new Mock<IConfigurationReader>();
            _cartService = _cartServiceMock.Object;
            _configurationReader = _configurationReaderMock.Object;
            _cartController = new CartController(_cartService, _configurationReader);
            _fixture = new Fixture();
        }
        
        #region Cart Method Tests

        [Fact]
        public async Task Cart_CartResultFailure_ReturnsErrorView()
        {
            //Arrange
            _cartServiceMock.Setup(item => item.GetLoggedUserCart()).ReturnsAsync(Result.Failure<CartResponseDto>(CartErrors.CartDoesNotExists));
            
            //Act
            IActionResult result = await _cartController.Cart();
            
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().Be(CartErrors.CartDoesNotExists.Description);
        }

        [Fact]
        public async Task Cart_CartResultSuccess_ReturnsView()
        {
            //Arrange
            CartResponseDto cartResponseDto = _fixture.Build<CartResponseDto>().Without(item => item.CartItems).Create();
            _cartServiceMock.Setup(item => item.GetLoggedUserCart()).ReturnsAsync(Result.Success(cartResponseDto));
            
            //Act
            IActionResult result = await _cartController.Cart();
            
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeEquivalentTo(cartResponseDto.ToViewModel(_configurationReader));
        }
        #endregion

    }
}

