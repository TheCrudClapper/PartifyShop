using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    [Authorize]
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
            var result = await _addressService.GetAddressForEdit();
            
            if (result.IsFailure)
                return View("Error", result.Error.Description);

            EditAddressViewModel viewModel = result.Value.ToViewModel();
            viewModel.CountriesSelectionList = (await _countriesService.GetCountriesSelectionList()).ConvertToSelectListItem();
            return PartialView("_EditAddressPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, EditAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CountriesSelectionList = (await _countriesService.GetCountriesSelectionList()).ConvertToSelectListItem();
                return PartialView("_EditAddressPartial", viewModel);
            }

            AddressDto dto = viewModel.ToDto();
            var result = await _addressService.Edit(id, dto);

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            return NoContent();
        }

    }
}
