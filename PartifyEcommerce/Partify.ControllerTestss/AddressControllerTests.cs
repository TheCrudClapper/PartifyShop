﻿using AutoFixture;
using CSOS.Core.DTO.AddressDto;
using CSOS.Core.DTO.UniversalDto;
using CSOS.Core.ResultTypes;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Controllers;
using CSOS.UI.Helpers;
using CSOS.UI.ViewModels.AddressViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CSOS.Tests
{
    public class AddressControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IAddressService _addressService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private AddressController _addressController = null!;
        private readonly ILogger<AddressController> _logger;
        public AddressControllerTests()
        {
            _fixture = new Fixture();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _addressServiceMock = new Mock<IAddressService>();
            _addressService = _addressServiceMock.Object;
            _countriesGetterService = _countriesGetterServiceMock.Object;
            _logger = Mock.Of<ILogger<AddressController>>();
        }
        #region EditUserAddress GET Method Tests

        public AddressController CreateController()
        {
            var controller = new AddressController(_addressService, _countriesGetterService, _logger);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }

        [Fact]
        public async Task Edit_AddressResultFailure_ReturnErrorView()
        {
            //Arrange
            _addressController = CreateController();
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
            _addressController = CreateController();
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
            _addressController = CreateController();
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
            _addressController = CreateController();
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
            _addressController = CreateController();
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
            //Arrange
            _addressController = CreateController();
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
