using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.Domain.ExternalServicesContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Helpers;
using CSOS.Core.ServiceContracts;
using CSOS.UI;
using CSOS.UI.Helpers;
using CSOS.UI.Helpers.Contracts;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly OfferViewModelInitializer _offerViewModelInitializer;
        private readonly IProductImageService _productImageService;
        private readonly IConfigurationReader _configurationReader;
        public OfferController(IOfferService offerService,
            IPictureHandlerService pictureHandlerService,
            OfferViewModelInitializer offerViewModelInitializer,
            IProductImageService productImageService,
            IConfigurationReader configurationReader)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
            _offerViewModelInitializer = offerViewModelInitializer;
            _productImageService = productImageService;
            _configurationReader = configurationReader;
        }
        
        [HttpGet]
        public async Task<IActionResult> AddOffer()
        {
            var viewModel = new AddOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel viewModel)
        {
            var pictureValidationResponse = PicturesValidatorHelper.ValidatePictureExtensions(viewModel.UploadedImages, _pictureHandlerService);
            if(pictureValidationResponse != null)
                ModelState.AddModelError("WrongFileType", pictureValidationResponse);

            if (!ModelState.IsValid)
            {
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            AddOfferDto dto = viewModel.ToAddOfferDto();
            var result = await _offerService.Add(dto);

            if(result.IsFailure)
                return View("Error", result.Error.Description);

            return RedirectToAction(nameof(AllUserOffers));
        }
        
        [HttpGet]
        public async Task<IActionResult> EditOffer([FromRoute] int id)
        {
            var response = await _offerService.GetOfferForEdit(id);

            if(response.IsFailure)
                return View("OfferDoesNotExist", id);

            var viewModel = response.Value.ToEditOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditOffer([FromRoute] int id, EditOfferViewModel viewModel)
        {
            var pictureValidationResponse = PicturesValidatorHelper.ValidatePictureExtensions(viewModel.UploadedImages, _pictureHandlerService);
            if (pictureValidationResponse != null)
                ModelState.AddModelError("WrongFileType", pictureValidationResponse);

            if (!ModelState.IsValid)
            {
                var images = await _productImageService.GetOfferPictures(id);
                viewModel.ExistingImagesUrls = images.ToSelectListItem();
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            var dto = viewModel.ToEditOfferDto();
            var result =  await _offerService.Edit(id, dto);

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            return RedirectToAction(nameof(AllUserOffers));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOffer([FromRoute]int id)
        {
            var result = await _offerService.DeleteOffer(id);

            if (result.IsFailure)
                return Json(new JsonResponseModel() { Message = result.Error.Description, Success = false });

            return Json(new JsonResponseModel() { Message = "Deleted Offer Successfully !", Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> AllUserOffers(string? title)
        {
            IEnumerable<UserOffersResponseDto> response = await _offerService.GetFilteredUserOffers(title);
            List<UserOffersViewModel> userOffers = response.ToViewModelCollection(_configurationReader);
            return View(userOffers);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowOffer([FromRoute] int id)
        {
            var response = await _offerService.GetOffer(id);

            if(response.IsFailure)
                return View("OfferDoesNotExist", id);

            var viewModel = response.Value.ToSingleOfferViewModel(_configurationReader);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferBrowser([FromQuery] OfferFilter filter)
        {
            OfferBrowserResponseDto response = await _offerService.GetFilteredOffers(filter);
            OfferBrowserViewModel viewModel = response.ToOfferBrowserViewModel(_configurationReader);
            return View(viewModel);
        }
    }
}
