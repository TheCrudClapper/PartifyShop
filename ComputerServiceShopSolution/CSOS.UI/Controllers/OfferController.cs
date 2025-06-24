using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Helpers;
using CSOS.UI.Helpers;
using CSOS.UI.Mappings.ToDto;
using CSOS.UI.Mappings.ToViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly OfferViewModelInitializer _offerViewModelInitializer;
        
        public OfferController(IOfferService offerService, IPictureHandlerService pictureHandlerService, OfferViewModelInitializer offerViewModelInitializer)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
            _offerViewModelInitializer = offerViewModelInitializer;
        }

        //OK
        [HttpGet]
        public async Task<IActionResult> AddOffer()
        {
            var viewModel = new AddOfferViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        //Change needed
        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel viewModel)
        {
            //Checking file extension using helper method
            var pictureValidationResponse = PicturesValidatorHelper.ValidatePictureExtensions(viewModel.UploadedImages, _pictureHandlerService);
            if(pictureValidationResponse != null)
                ModelState.AddModelError("WrongFileType", pictureValidationResponse);

            //Checking fields
            if (!ModelState.IsValid)
            {
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            AddOfferDto dto = await viewModel.ToDto(_pictureHandlerService);
            var result = await _offerService.Add(dto);

            if(result.IsFailure)
                return View("Error", result.Error.Description);

            return RedirectToAction(nameof(AllUserOffers));
        }

        //OK
        [HttpGet]
        public async Task<IActionResult> EditOffer([FromRoute] int id)
        {
            var response = await _offerService.GetOfferForEdit(id);

            if(response.IsFailure)
                return View("OfferNotFound", id);

            var viewModel = response.Value.ToViewModel();
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
                var images = await _offerService.GetOfferPictures(id);

                viewModel.ExistingImagesUrls = images.Select(item => new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                })
                .ToList();

                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            var dto = new EditOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                StockQuantity = viewModel.StockQuantity,

                //if uploaded images are empty, submit empty list
                UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages ?? new List<IFormFile>()),
                ImagesToDelete = viewModel.ImagesToDelete,
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
            };
            await _offerService.Edit(id, dto);
            return RedirectToAction(nameof(AllUserOffers));
        }

        //Changes needed
        [HttpPost]
        public async Task<IActionResult> DeleteOffer([FromRoute]int id)
        {
            var result = await _offerService.DeleteOffer(id);

            //or json object
            if(result.IsFailure)
                return StatusCode(500);

            return NoContent();
        }

        //OK
        [HttpGet]
        public async Task<IActionResult> AllUserOffers(string? title)
        {
            List<UserOffersResponseDto> response = await _offerService.GetFilteredUserOffers(title);
            List<UserOffersViewModel> userOffers = response.ToViewModelCollection();
            return View(userOffers);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowOffer([FromRoute] int id)
        {
            var response = await _offerService.GetOffer(id);

            if(response.IsFailure)
                return View("OfferNotFound", id);

            var viewModel = response.Value.ToViewModel();
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferBrowser([FromQuery] OfferFilter filter)
        {
            OfferBrowserResponseDto response = await _offerService.GetFilteredOffers(filter);
            OfferBrowserViewModel viewModel = response.ToViewModel();
            return View(viewModel);
        }
    }
}
