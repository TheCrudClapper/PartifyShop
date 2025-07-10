using AutoFixture;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.DTO.Responses.Cart;
using CSOS.Core.ErrorHandling;
using CSOS.UI;
using CSOS.UI.Helpers.Contracts;
using CSOS.UI.Mappings.ToViewModel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            _cartServiceMock.Setup(item => item.GetLoggedUserCart())
                .ReturnsAsync(Result.Failure<CartResponseDto>(CartErrors.CartDoesNotExists));

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
            CartResponseDto cartResponseDto =
                _fixture.Build<CartResponseDto>().Without(item => item.CartItems).Create();
            _cartServiceMock.Setup(item => item.GetLoggedUserCart()).ReturnsAsync(Result.Success(cartResponseDto));

            //Act
            IActionResult result = await _cartController.Cart();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeEquivalentTo(cartResponseDto.ToViewModel(_configurationReader));
        }

        #endregion

        #region AddToCart Method Tests

        [Fact]
        public async Task AddToCart_AddingToCartFailure_ReturnsErrorJson()
        {
            //Arrange
            var error = OfferErrors.OfferDoesNotExist;
            _cartServiceMock.Setup(item => item.AddToCart(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.AddToCart(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel
                {
                    Success = false,
                    Message = $"Error: {OfferErrors.OfferDoesNotExist.Description}"
                });
        }

        [Fact]
        public async Task AddToCart_AddingToCartSuccess_ReturnsSuccessJson()
        {
            //Arrange
            _cartServiceMock.Setup(item => item.AddToCart(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _cartController.AddToCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel
                {
                    Success = true,
                    Message = "Success: Item Successfully Added to Cart",
                });
        }

        #endregion

        #region DeleteFromCart Method Tests
        
        [Fact]
        public async Task DeleteFromCart_DeleteFromCartFailure_ReturnsErrorJson()
        {
            //Arrange
            var error = CartItemErrors.CartItemDoesNotExists;
            _cartServiceMock.Setup(item => item.DeleteFromCart(It.IsAny<int>())).ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.DeleteFromCart(It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = false,
                    Message = $"Error: {error.Description}"
                });
        }

        [Fact]
        public async Task DeleteFromCart_DeleteFromCartSuccess_ReturnsSuccessJson()
        {
            //Arrange
            _cartServiceMock.Setup(item => item.DeleteFromCart(It.IsAny<int>())).ReturnsAsync(Result.Success);
            
            //Act
            IActionResult result = await _cartController.DeleteFromCart(It.IsAny<int>());
            
            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = true,
                    Message = "Success: Item removed from cart successfully!",
                });
        }
        #endregion

        #region UpdateQuantityInCart Method Tests
        [Fact]
        public async Task UpdateQuantityInCart_UpdateCartItemQuantityFailure_ReturnErrorJson()
        {
            //Arrange
            var error = CartItemErrors.CartItemDoesNotExists;
            _cartServiceMock.Setup(item => item.UpdateCartItemQuantity(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Result.Failure(error));

            //Act
            IActionResult result = await _cartController.UpdateQuantityInCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = false,
                    Message = $"Error: {error.Description}",
                });
        }

        [Fact]
        public async Task UpdateQuantityInCart_UpdateCartItemQuantitySuccess_ReturnSuccessJson()
        {
            //Arrange
            _cartServiceMock.Setup(item => item.UpdateCartItemQuantity(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _cartController.UpdateQuantityInCart(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<JsonResult>()
                .Which.Value.Should().BeOfType<JsonResponseModel>()
                .Subject.Should().BeEquivalentTo(new JsonResponseModel()
                {
                    Success = true,
                    Message = "Updated cart successfully!"
                });
        }
        #endregion

        #region GetCartItemsCount Method Tests
        [Fact]
        public void GetCartItemsCount_ReturnViewComponent()
        {
            //Act
            IActionResult result = _cartController.GetCartItemsCount();

            //Assert
            result.Should().BeOfType<ViewComponentResult>();
        }
        #endregion
    }
}