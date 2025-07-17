using AutoFixture;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CSOS.Tests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly IOfferService _offerService;
        private readonly Mock<IOfferService> _offerServiceMock;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly Mock<ICategoryGetterService> _categoryGetterServiceMock;
        private readonly IConfigurationReader _configurationReader;
        private readonly Mock<IConfigurationReader> _configuartionReaderMock;
        private readonly HomeController homeController;
        private readonly IFixture _fixture;

        public HomeControllerTests()
        {
            _fixture = new Fixture();
            _offerServiceMock = new Mock<IOfferService>();
            _categoryGetterServiceMock = new Mock<ICategoryGetterService>();
            _configuartionReaderMock = new Mock<IConfigurationReader>();
            _categoryGetterService = _categoryGetterServiceMock.Object;
            _offerService = _offerServiceMock.Object;
            _configurationReader = _configuartionReaderMock.Object;
        }
        private HomeController CreateController()
        {
            var homeController = new HomeController(_offerService, _categoryGetterService, _configurationReader);
            return homeController;
        }
        #region Index Method Tests
        [Fact]
        public async Task Index_ReturnViewsWithCorrectViewModel()
        {
            //Arrange
            IEnumerable<MainPageCardResponseDto> offers = _fixture.CreateMany<MainPageCardResponseDto>();
            IEnumerable<SelectListItemDto> categories = _fixture.CreateMany<SelectListItemDto>();

            _offerServiceMock.Setup(item => item.GetIndexPageOffers()).ReturnsAsync(offers);

            _categoryGetterServiceMock.Setup(item => item.GetProductCategoriesAsSelectList()).ReturnsAsync(categories);

            _categoryGetterServiceMock.Setup(item => item.GetProductCategoriesAsMainPageCardResponseDto()).ReturnsAsync(_fixture.CreateMany<MainPageCardResponseDto>());

            _offerServiceMock.Setup(item => item.GetDealsOfTheDay()).ReturnsAsync(offers);

            var homeController = CreateController();

            //Act
            var result = await homeController.Index();

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<IndexPageViewModel>().Subject;

            model.Cards.Should().NotBeNull();
            model.Cards.Should().HaveCount(offers.Count());
            model.BestDeals.Should().NotBeNull();
            model.BestDeals.Should().HaveCount(offers.Count());
            model.Categories.Should().NotBeNull();
            model.Categories.Should().HaveCount(categories.Count());
            model.CategoriesSlider.Should().NotBeNull();
            model.CategoriesSlider.Should().HaveCount(3);
        }
        #endregion

        #region Privacy Method Tests
        [Fact]
        public void Privacy_ReturnsView()
        {
            //Arrange
            var controller = CreateController();

            //Act
            IActionResult result = controller.Privacy();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
        #endregion

        #region AboutUs Method Tests
        [Fact]
        public void AboutUs_ReturnsView()
        {
            //Arrange
            var controller = CreateController();

            //Act
            IActionResult result = controller.AboutUs();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
        #endregion

        #region Error Method Tests
        [Fact]
        public void Error_WithExceptionHandler_SetsViewBagError()
        {
            // Arrange
            var controller = CreateController();

            var exceptionFeature = new Mock<IExceptionHandlerPathFeature>();
            exceptionFeature.Setup(f => f.Error).Returns(new Exception("Test exception message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set(exceptionFeature.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Error();

            // Assert
            Assert.Equal("Test exception message", controller.ViewBag.Error);
        }

        [Fact]
        public void Error_WithoutExceptionHandler_ReturnsViewWithoutError()
        {
            // Arrange
            var homeController = CreateController();
            homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            };

            // Act
            var result = homeController.Error();

            // Assert
            result.Should().BeOfType<ViewResult>();
            Assert.Null(homeController.ViewBag.Error);
        }
        #endregion
    }
}
