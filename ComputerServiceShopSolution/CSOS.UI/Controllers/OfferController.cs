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
            //Checking file extensions
            if (viewModel.UploadedImages != null)
            {
                string response = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);
                if (response != "OK")
                {
                    ModelState.AddModelError("WrongFileType", response!);
                }
            }
            //Checking fields
            if (!ModelState.IsValid)
            {
                await _offerViewModelInitializer.InitializeAllAsync(viewModel);
                return View(viewModel);
            }

            AddOfferDto dto = await viewModel.ToDto(_pictureHandlerService);
            await _offerService.Add(dto);
            return RedirectToAction(nameof(AllUserOffers));
        }

        [HttpGet]
        public async Task<IActionResult> EditOffer([FromRoute] int id)
        {
            if (!await _offerService.DoesOfferExist(id))
            {
                return View("OfferNotFound", id);
            }
            var response = await _offerService.GetOfferForEdit(id);
            var viewModel = response.ToViewModel();
            await _offerViewModelInitializer.InitializeAllAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditOffer([FromRoute] int id, EditOfferViewModel viewModel)
        {
            if (viewModel.UploadedImages != null)
            {
                string response = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);
                if (response != "OK")
                {
                    ModelState.AddModelError("WrongFileType", response!);
                }
            }

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
            return RedirectToAction("AllUserOffers");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOffer([FromRoute]int id)
        {
            try
            {
                await _offerService.DeleteOffer(id);
                return Ok();
            }
            catch(UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllUserOffers(string? title)
        {
            List<UserOffersResponseDto> response = await _offerService.GetFilteredUserOffers(title);
            List<UserOffersViewModel> userOffers = response.ToViewModelCollection();

            if (!userOffers.Any())
            {
                if (string.IsNullOrEmpty(title))
                {
                    ViewBag.Message = "You don't have any active offers yet";
                }
                else
                {
                    ViewBag.Message = "No offer matched your search phrase";
                }
            }
            return View(userOffers);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowOffer([FromRoute] int id)
        {
            if (!await _offerService.DoesOfferExist(id))
            {
                return View("OfferNotFound", id);
            }
            var response = await _offerService.GetOffer(id);
            var viewModel = response.ToViewModel();
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
