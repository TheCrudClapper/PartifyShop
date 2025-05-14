using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly ICountriesService _countriesService;
        public AddressController(IAddressService addressService, ICountriesService countriesService)
        {
            _addressService = addressService;
            _countriesService = countriesService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]int id)
        {
            var address = await _addressService.GetAddressForEdit();
            address.CountriesSelectionList = await _countriesService.GetCountriesSelectionList();
            return PartialView("_EditAddressPartial", address);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, EditAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CountriesSelectionList = await _countriesService.GetCountriesSelectionList();
                return PartialView("_EditAddressPartial", viewModel);
            }
            var dto = new AddressDto()
            {
                CountryId = int.Parse(viewModel.SelectedCountry),
                HouseNumber = viewModel.HouseNumber,
                Place = viewModel.Place,
                PostalCity = viewModel.PostalCity,
                PostalCode = viewModel.PostalCode,
                Street = viewModel.Street,
            };
            await _addressService.Edit(id, dto);

            return Ok();
        }

    }
}
