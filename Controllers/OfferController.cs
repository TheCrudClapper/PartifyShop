using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels;
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
            //Initializing list in view
            var viewModel = new AddOfferViewModel();
            await InitializeViewModelsCollections(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel viewModel)
        {

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            string response = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);

            if (response != "OK")
            {
                ModelState.AddModelError("ImagesError", response!);
            }
            if (!ModelState.IsValid)
            {
                await InitializeViewModelsCollections(viewModel);
                return View(viewModel);
            }
            var dto = new AddOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.OfferVisibility,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                StockQuantity = viewModel.StockQuantity,
                UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages),
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
            };
            await _offerService.Add(dto);
            return RedirectToAction("AllUserOffers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOffer([FromRoute]int id)
        {
            await _offerService.DeleteOffer(id);
            return RedirectToAction("AllUserOffers");
        }

        [HttpGet]
        public async Task<IActionResult>AllUserOffers()
        {
            IEnumerable<UserOffersViewModel> userOffers = await _offerService.GetUserOffers();
            return View(userOffers);
        }

        [HttpGet]
        public async Task<IActionResult> ShowOffer([FromRoute]int id)
        {
            var viewModel = await _offerService.GetOffer(id);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferBrowser()
        {
            IEnumerable<OfferBrowserViewModel> allOffers = await _offerService.GetAllOffers();
            return View(allOffers);
        }
        public IActionResult ShowOffer()
        {
            return View();
        }
        public async Task InitializeViewModelsCollections(AddOfferViewModel viewModel)
        {
            viewModel.DeliveryTypes = await _offerService.GetParcelLockerDeliveryTypes();
            viewModel.ProductConditionsSelectList = await _offerService.GetProductConditions();
            viewModel.ProductCategoriesSelectionList = await _offerService.GetProductCategories();
            viewModel.OtherDeliveriesSelectedList = await _offerService.GetOtherDeliveryTypes();
        }
    }
}
