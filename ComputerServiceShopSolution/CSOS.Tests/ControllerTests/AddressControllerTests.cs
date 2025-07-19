using AutoFixture;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.AccountDto;
using CSOS.Core.DTO.AddressDto;
using CSOS.Core.ErrorHandling;
using CSOS.Core.ServiceContracts;
using CSOS.UI;
using CSOS.UI.Controllers;
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
        #region EditUserAddress GET Method Tests

        [Fact]
        public async Task Edit_AddressResultFailure_ReturnErrorView()
        {
            //Arrange
            _addressServiceMock.Setup(item => item.GetUserAddressForEdit()).ReturnsAsync(Result.Failure<AddressResponse>(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _addressController.Edit(It.IsAny<int>());

            //Assert
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().Be(AddressErrors.AddressNotFound.Description);
        }

        [Fact]
        public async Task Edit_AddressFound_ReturnPartialView()
        {
            //Arrange
            List<SelectListItemDto> countries = _fixture.CreateMany<SelectListItemDto>().ToList();
            AddressResponse dto = _fixture.Create<AddressResponse>();
            _addressServiceMock.Setup(item => item.GetUserAddressForEdit()).ReturnsAsync(dto);
            _countriesGetterServiceMock.Setup(item => item.GetCountriesSelectionList()).ReturnsAsync(countries);

            //Act
            IActionResult result = await _addressController.Edit(It.IsAny<int>());

            //Assert
            PartialViewResult viewResult = result.Should().BeOfType<PartialViewResult>().Subject;
            viewResult.Model.Should().BeOfType<EditAddressViewModel>();
            viewResult.Model.Should().NotBeNull();
        }
        #endregion

        #region EditUserAddress POST Method Tests
        [Fact]
        public async Task Edit_EditAddresSuccessResult_ReturnSuccessJson()
        {
            //Arrange
            EditAddressViewModel viewModel = _fixture.Build<EditAddressViewModel>()
                .With(item => item.SelectedCountry, "21")
                .Create();

            _addressServiceMock.Setup(item => item.EditUserAddress(It.IsAny<AddressUpdateRequest>())).ReturnsAsync(Result.Success);

            //Act
            IActionResult result = await _addressController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
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

            _addressServiceMock.Setup(item => item.EditUserAddress(It.IsAny<AddressUpdateRequest>())).ReturnsAsync(Result.Failure(AddressErrors.AddressNotFound));

            //Act
            IActionResult result = await _addressController.Edit(viewModel);

            //Assert
            JsonResult jsonResult = result.Should().BeOfType<JsonResult>().Subject;
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
            IActionResult result = await _addressController.Edit(viewModel);

            //Assert
            PartialViewResult partialView = result.Should().BeOfType<PartialViewResult>().Subject;
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
            IActionResult result = await _addressController.Edit(viewModel);

            //Assert
            PartialViewResult partialView = result.Should().BeOfType<PartialViewResult>().Subject;
            partialView.Should().NotBeNull();
            partialView.Model.Should().Be(viewModel);
            partialView.ViewName.Should().Be("AccountPartials/_AddressChangeForm");
        }
        #endregion
    }
}
