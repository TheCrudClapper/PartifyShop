using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.Helpers;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.OfferViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly OfferViewModelInitializer _offerViewModelInitializer;
        private readonly IProductImageService _productImageService;
        private readonly IConfigurationReader _configurationReader;
        private readonly ISortingOptionService _sortingOptionService;
        public OfferController(IOfferService offerService,
            IDeliveryTypeGetterService deliveryTypeGetterService,
            IPictureHandlerService pictureHandlerService,
            OfferViewModelInitializer offerViewModelInitializer,
            IProductImageService productImageService,
            ISortingOptionService sortingOptionService,
            IConfigurationReader configurationReader)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
            _offerViewModelInitializer = offerViewModelInitializer;
            _productImageService = productImageService;
            _sortingOptionService = sortingOptionService;
            _configurationReader = configurationReader;
            _deliveryTypeGetterService = deliveryTypeGetterService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new AddOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddOfferViewModel viewModel)
        {
            var pictureValidationResponse = PicturesValidatorHelper.ValidatePictureExtensions(viewModel.UploadedImages, _pictureHandlerService);
            if(pictureValidationResponse != null)
                ModelState.AddModelError("WrongFileType", pictureValidationResponse);

            if (!ModelState.IsValid)
            {
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            OfferAddRequest dto = viewModel.ToAddOfferDto();
            var result = await _offerService.Add(dto);

            if(result.IsFailure)
                return View("Error", result.Error.Description);

            return RedirectToAction(nameof(UserOffers));
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var response = await _offerService.GetOfferForEdit(id);

            if(response.IsFailure)
                return View("Error", response.Error.Description);

            var viewModel = response.Value.ToEditOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOfferViewModel viewModel)
        {
            var pictureValidationResponse = PicturesValidatorHelper.ValidatePictureExtensions(viewModel.UploadedImages, _pictureHandlerService);
            if (pictureValidationResponse != null)
                ModelState.AddModelError("WrongFileType", pictureValidationResponse);

            if (!ModelState.IsValid)
            {
                var images = await _productImageService.GetOfferPicturesAsync(viewModel.Id);
                viewModel.ExistingImagesUrls = images.ToSelectListItem();
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            var dto = viewModel.ToEditOfferDto();
            var result =  await _offerService.Edit(dto);

            if (result.IsFailure)
                return View("Error", result.Error.Description);

            return RedirectToAction(nameof(UserOffers));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result = await _offerService.DeleteOffer(id);

            if (result.IsFailure)
                return Json(new JsonResponseModel() { Message = result.Error.Description, Success = false });

            return Json(new JsonResponseModel() { Message = "Deleted Offer Successfully !", Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> UserOffers(string? title)
        {
            IEnumerable<UserOfferResponse> response = await _offerService.GetFilteredUserOffers(title);
            IEnumerable<UserOffersViewModel> userOffers = response
                .Select(item => item.ToUserOffersViewModel(_configurationReader));
            
            return View(userOffers);
        }

        [HttpGet]
        public async Task<IActionResult> FilterUserOffers(string? title)
        {
            IEnumerable<UserOfferResponse> response = await _offerService.GetFilteredUserOffers(title);
            IEnumerable<UserOffersViewModel> viewModel =  response
                .Select(item  => item.ToUserOffersViewModel(_configurationReader));
            return PartialView("OfferPartials/_UserOfferListPartial",  viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var response = await _offerService.GetOffer(id);

            if(response.IsFailure)
                return View("Error", response.Error.Description);

            var viewModel = response.Value.ToOfferDetailsViewModel(_configurationReader);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] OfferFilter filter)
        {
            IEnumerable<OfferIndexResponse> filteredOffers = await _offerService.GetFilteredOffers(filter);
            OfferBrowserViewModel viewModel = new OfferBrowserViewModel()
            {
                Items = filteredOffers.Select(item => item.ToOfferIndexItemViewModel(_configurationReader))
                    .ToList(),
                
                DeliveryOptions = (await _deliveryTypeGetterService.GetAllDeliveryTypesAsSelectionList())
                    .ToSelectListItem(),
                
                SortingOptions = _sortingOptionService.GetSortingOptions()
                    .ToSelectListItem(),
                
                Filter = filter,  
            };
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FilterOffers([FromQuery] OfferFilter filter)
        {
            IEnumerable<OfferIndexResponse> filteredOffers = await _offerService.GetFilteredOffers(filter);
            var viewModel = filteredOffers.Select(item => item.ToOfferIndexItemViewModel(_configurationReader));
            return PartialView("OfferPartials/_OfferListPartial", viewModel);
        }
    }
}
