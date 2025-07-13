using AutoFixture;
using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Controllers;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.Core.ErrorHandling;
using CSOS.UI;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CSOS.Tests.ControllerTests
{
    public class AddressControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IAddressService _addressService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly AddressController _addressController;
        public AddressControllerTests()
        {
            _fixture = new Fixture();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _addressServiceMock = new Mock<IAddressService>();
            _addressService = _addressServiceMock.Object;
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _addressController = new AddressController(_addressService, _countriesGetterService);
        }
        #region Edit GET Method Tests

        [Fact]
        public async Task Edit_AddressResultFailure_ReturnErrorView()
        {
            //Arrange
            _addressServiceMock.Setup(item => item.GetAddressForEdit()).ReturnsAsync(Result.Failure<EditAddressResponseDto>(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _addressController.Edit(It.IsAny<int>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.Model.Should().Be(AddressErrors.AddressNotFound.Description);
        }

        [Fact]
        public async Task Edit_AddressFound_ReturnPartialView()
        {
            //Arrange
            List<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>().ToList();
            EditAddressResponseDto dto = _fixture.Create<EditAddressResponseDto>();
            _addressServiceMock.Setup(item => item.GetAddressForEdit()).ReturnsAsync(dto);
            _countriesGetterServiceMock.Setup(item => item.GetCountriesSelectionList()).ReturnsAsync(countries);

            //Act
            IActionResult result = await _addressController.Edit(It.IsAny<int>());

            //Assert
            PartialViewResult viewResult = Assert.IsType<PartialViewResult>(result);
            viewResult.Model.Should().BeOfType<EditAddressViewModel>();
            viewResult.Model.Should().NotBeNull();
        }
        #endregion

        #region Edit POST Method Tests
        [Fact]
        public async Task Edit_EditAddresSuccessResult_ReturnSuccessJson()
        {
            //Arrange
            EditAddressViewModel viewModel = _fixture.Build<EditAddressViewModel>()
                .With(item => item.SelectedCountry, "21")
                .Create();

            _addressServiceMock.Setup(item => item.Edit(It.IsAny<int>(), It.IsAny<AddressDto>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _addressController.Edit(viewModel.Id, viewModel);

            //Assert
            JsonResult jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Should().NotBeNull();
            jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject.Message.Should().Be("Address updated successfully !");
            jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject.Success.Should().Be(true);
        }

        [Fact]
        public async Task Edit_EditAddressFailureResult_ReturnErrorJson()
        {
            //Arrange
            EditAddressViewModel viewModel = _fixture.Build<EditAddressViewModel>()
                .With(item => item.SelectedCountry, "21")
                .Create();

            _addressServiceMock.Setup(item => item.Edit(It.IsAny<int>(), It.IsAny<AddressDto>())).ReturnsAsync(Result.Failure(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _addressController.Edit(viewModel.Id, viewModel);

            //Assert
            JsonResult jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Should().NotBeNull();
            jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject.Message.Should().Be(AddressErrors.AddressNotFound.Description);
            jsonResult.Value.Should().BeOfType<JsonResponseModel>().Subject.Success.Should().Be(false);
        }

        [Fact]
        public async Task Edit_ModelStateInvalidSourceAddOrder_ReturnPartialView()
        {
            //Arrange
            List<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>().ToList();
            EditAddressViewModel viewModel = _fixture.Build<EditAddressViewModel>()
               .With(item => item.SelectedCountry, "21")
               .With(item => item.Source, "AddOrder")
               .Create();

            _countriesGetterServiceMock.Setup(item => item.GetCountriesSelectionList()).ReturnsAsync(countries);
            _addressController.ModelState.AddModelError("TestError", "ErrorTest");

            //Act
            IActionResult result = await _addressController.Edit(viewModel.Id, viewModel);

            //Assert
            PartialViewResult partialView = Assert.IsType<PartialViewResult>(result);
            partialView.Should().NotBeNull();
            partialView.Model.Should().Be(viewModel);
            partialView.ViewName.Should().Be("_EditAddressPartial");
        }

        [Fact]
        public async Task Edit_ModelStateInvalidSorceAccountDetails_ReturnsPartialView()
        {
            List<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>().ToList();
            EditAddressViewModel viewModel = _fixture.Build<EditAddressViewModel>()
               .With(item => item.SelectedCountry, "21")
               .With(item => item.Source, "AccountDetails")
               .Create();

            _countriesGetterServiceMock.Setup(item => item.GetCountriesSelectionList()).ReturnsAsync(countries);
            _addressController.ModelState.AddModelError("TestError", "ErrorTest");

            //Act
            IActionResult result = await _addressController.Edit(viewModel.Id, viewModel);

            //Assert
            PartialViewResult partialView = Assert.IsType<PartialViewResult>(result);
            partialView.Should().NotBeNull();
            partialView.Model.Should().Be(viewModel);
            partialView.ViewName.Should().Be("AccountPartials/_AddresChangePartial");
        }
        #endregion
    }
}
