using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
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
            EditAddressResponseDto response = await _addressService.GetAddressForEdit();
            EditAddressViewModel viewModel = response.ToViewModel();
            return PartialView("_EditAddressPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, EditAddressViewModel viewModel)
        {
            List<SelectListItemDto> response = await _countriesService.GetCountriesSelectionList();
            if (!ModelState.IsValid)
            {
                viewModel.CountriesSelectionList = response.ConvertToSelectListItem();
                return PartialView("_EditAddressPartial", viewModel);
            }

            AddressDto dto = viewModel.ToDto();
            await _addressService.Edit(id, dto);

            return Ok();
        }

    }
}
