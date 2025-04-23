using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPictureHandlerService _pictureHandlerService;
        public OfferController(IOfferService offerService, IPictureHandlerService pictureHandlerService)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
        }

        [HttpGet]
        public async Task<IActionResult> AddOffer()
        {
            var viewModel = new AddOfferViewModel();
            await InitializeViewModelsCollections(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel viewModel)
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
                await InitializeViewModelsCollections(viewModel);
                return View(viewModel);
            }
            var dto = new AddOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                StockQuantity = viewModel.StockQuantity,
                UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages!),
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
            };
            await _offerService.Add(dto);
            return RedirectToAction("AllUserOffers");
        }

        [HttpGet]
        public async Task<IActionResult> EditOffer([FromRoute] int id)
        {
            if (!await _offerService.DoesOfferExist(id))
            {
                return View("OfferNotFound", id);
            }
            var viewModel = await _offerService.GetOfferForEdit(id);
            await InitializeViewModelsCollections(viewModel);
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

            //int existingImagesCount = (viewModel.ExistingImagesUrls?.Count ?? 0) - (viewModel.ImagesToDelete?.Count ?? 0);
            //int newImagesCount = viewModel.UploadedImages?.Count ?? 0;

            //if (existingImagesCount + newImagesCount <= 0)
            //{
            //    ModelState.AddModelError("ImagesDeletion", "You must have at least one image for the offer.");
            //}

            if (!ModelState.IsValid)
            {
                viewModel.ExistingImagesUrls = await _offerService.GetOfferPictures(id);
                await InitializeViewModelsCollections(viewModel);
                return View(viewModel);
            }

            var dto = new EditOfferDto()
            {
                Id = viewModel.Id,
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
            await _offerService.Edit(dto);
            return RedirectToAction("AllUserOffers");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOffer([FromRoute] int id)
        {
            await _offerService.DeleteOffer(id);
            return RedirectToAction("AllUserOffers");
        }

        [HttpGet]
        public async Task<IActionResult> AllUserOffers()
        {
            IEnumerable<UserOffersViewModel> userOffers = await _offerService.GetUserOffers();
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
            var viewModel = await _offerService.ShowOffer(id);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferBrowser([FromQuery] OfferFilter filter)
        {
            OfferBrowserViewModel offers = await _offerService.GetFilteredOffers(filter);
            return View(offers);
        }
        public async Task InitializeViewModelsCollections<ViewModelType>(ViewModelType viewModel) where ViewModelType : BaseOfferViewModel
        {
            viewModel.ParcelLockerDeliveriesList = await _offerService.GetParcelLockerDeliveryTypes();
            viewModel.ProductConditionsSelectList = await _offerService.GetProductConditions();
            viewModel.ProductCategoriesSelectionList = await _offerService.GetProductCategoriesAsSelectList();
            viewModel.OtherDeliveriesSelectedList = await _offerService.GetOtherDeliveryTypes();
        }
    }
}
