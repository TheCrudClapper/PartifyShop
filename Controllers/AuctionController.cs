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
            viewModel.DeliveryTypes = await _offerService.GetParcelLockerDeliveryTypes();
            viewModel.ProductConditionsSelectList = await _offerService.GetProductConditions();
            viewModel.ProductCategoriesSelectionList = await _offerService.GetProductCategories();
            viewModel.OtherDeliveriesSelectedList = await _offerService.GetOtherDeliveryTypes();

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            string response = _pictureHandlerService.CheckFileExtensions(viewModel.UploadedImages);

            if (response != "OK")
            {
                ModelState.AddModelError("ImagesError", response);
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.UploadedImagesUrls = _pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages);
            await _offerService.Add(viewModel);
            return RedirectToAction("AllUserAuctions");
        }
        public IActionResult AllUserAuctions()
        {
            return View();
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
