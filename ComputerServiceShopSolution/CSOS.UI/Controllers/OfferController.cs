using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                var images = await _offerService.GetOfferPictures(id);

                viewModel.ExistingImagesUrls = images.Select(item => new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                })
                .ToList();

                await InitializeViewModelsCollections(viewModel);
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
            List<UserOffersViewModel> userOffers = response.Select(item => new UserOffersViewModel()
            {
                DateCreated = item.DateCreated,
                Id = item.Id,
                ImageUrl = item.ImageUrl,
                ProductCategory = item.ProductCategory,
                Price = item.Price,
                ProductCondition = item.ProductCondition,
                ProductName = item.ProductName,
                ProductStatus = item.ProductStatus,
                StockQuantity = item.StockQuantity,
            })
            .ToList();

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
            var viewModel = await _offerService.GetOffer(id);
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferBrowser([FromQuery] OfferFilter filter)
        {
            OfferBrowserResponseDto response = await _offerService.GetFilteredOffers(filter);
            OfferBrowserViewModel viewModel = new OfferBrowserViewModel()
            {
                DeliveryOptions = response.DeliveryOptions.Select(item => new SelectListItem()
                {
                    Value = item.Value,
                    Text = item.Text,
                }).ToList(),
                Filter = response.Filter
            };
            return View(viewModel);
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
