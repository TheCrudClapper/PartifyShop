using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.Models.Services;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Controllers
{
    public class AuctionController : Controller
    {
        private readonly OfferService _offerService;
        public AuctionController(DatabaseContext context)
        {
            _offerService = new OfferService(context);
        }
        public async Task<IActionResult> AddAuction()
        {
            //Initializing list in view
            var viewModel = new OfferViewModel()
            {
                DeliveryTypes = await _offerService.GetDeliveryTypes(),
                ProductConditionsSelectList = await _offerService.GetProductConditions(),
                ProductCategoriesSelectionList = await _offerService.GetProductCategories(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddAuction(OfferViewModel viewModel)
        {
            viewModel.DeliveryTypes = await _offerService.GetDeliveryTypes();
            viewModel.ProductConditionsSelectList = await _offerService.GetProductConditions();
            viewModel.ProductCategoriesSelectionList = await _offerService.GetProductCategories();

            string[] allowedExtensions = new[] { ".jpg", "jpeg", "png" };

            if (viewModel.UploadedImages != null)
            {
                foreach (var image in viewModel.UploadedImages)
                {
                    var extension = Path.GetExtension(image.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("UploadedImages", $"Images should be only in format {string.Join(",", allowedExtensions)}");
                        return View(viewModel);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var imagePaths = new List<string>();
            if (viewModel.UploadedImages != null && viewModel.UploadedImages.Count > 0)
            {
                foreach (var file in viewModel.UploadedImages)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                            + "_" + Guid.NewGuid()
                            + Path.GetExtension(file.FileName);

                        var filePath = Path.Combine("wwwroot/offer-images/", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        //this need to be added to database
                        imagePaths.Add($"/offer{fileName}");
                    }
                }
            }
           
            

            
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
