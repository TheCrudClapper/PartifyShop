using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Requests;
using CSOS.Core.ServiceContracts;
using CSOS.UI;
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
        private readonly ICountriesGetterService _countriesGetterService;
        public AddressController(IAddressService addressService, ICountriesGetterService countriesGetterService)
        {
            _addressService = addressService;
            _countriesGetterService = countriesGetterService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var result = await _addressService.GetAddressForEdit();

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            EditAddressViewModel viewModel = result.Value.ToViewModel();
            viewModel.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList()).ToSelectListItem();
            return PartialView("_EditAddressPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, EditAddressViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CountriesSelectionList = (await _countriesGetterService.GetCountriesSelectionList())
                    .ToSelectListItem();


                return viewModel.Source switch
                {
                    "AddOrder" => PartialView("_EditAddressPartial", viewModel),
                    "AccountDetails" => PartialView("AccountPartials/_AddresChangePartial", viewModel),
                    _ => Json(new JsonResponseModel() { Message = "Something went wrong", Success = false })
                };
            }

            AddressDto dto = viewModel.ToDto();
            var result = await _addressService.Edit(id, dto);

            if (result.IsFailure)
                return Json(new JsonResponseModel() { Message = result.Error.Description, Success = false });

            return Json(new JsonResponseModel() { Message = "Address updated successfully !", Success = true });
        }

    }
}
