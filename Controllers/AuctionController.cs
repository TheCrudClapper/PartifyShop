using ComputerServiceOnlineShop.Models.Abstractions;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IPictureHandlerService _pictureHandlerService;
        public AuctionController(IOfferService offerService, IPictureHandlerService pictureHandlerService)
        {
            _offerService = offerService;
            _pictureHandlerService = pictureHandlerService;
        }

        [HttpGet]
        public async Task<IActionResult> AddAuction()
        {
            //Initializing list in view
            var viewModel = new OfferViewModel()
            {
                DeliveryTypes = await _offerService.GetParcelLockerDeliveryTypes(),
                ProductConditionsSelectList = await _offerService.GetProductConditions(),
                ProductCategoriesSelectionList = await _offerService.GetProductCategories(),
                OtherDeliveriesSelectedList = await _offerService.GetOtherDeliveryTypes(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuction(OfferViewModel viewModel)
        {

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            string response = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);

            if (response != "OK")
            {
                ModelState.AddModelError("ImagesError", response!);
            }
            if (!ModelState.IsValid)
            {
                viewModel.DeliveryTypes = await _offerService.GetParcelLockerDeliveryTypes();
                viewModel.ProductConditionsSelectList = await _offerService.GetProductConditions();
                viewModel.ProductCategoriesSelectionList = await _offerService.GetProductCategories();
                viewModel.OtherDeliveriesSelectedList = await _offerService.GetOtherDeliveryTypes();
                return View(viewModel);
            }

            viewModel.UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages);
            await _offerService.Add(viewModel);
            return RedirectToAction("AllUserOffers");
        }
        public async Task<IActionResult> AllUserOffersAsync()
        {
            List<UserOffersViewModel> userOffers = await _offerService.GetUserOffers();
            return View(userOffers);
        }
        public IActionResult ShowAuction()
        {
            return View();
        }
        public IActionResult AuctionBrowser()
        {
            return View();
        }
    }
}
