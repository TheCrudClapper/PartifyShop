using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace ComputerServiceOnlineShop.Services
{
    public class OfferService : IOfferService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IAccountService _accountService;
        public OfferService(DatabaseContext databaseContext, IAccountService accountService)
        {
            _databaseContext = databaseContext;
            _accountService = accountService;
        }
        public async Task Add(AddOfferDto dto)
        {
            Guid userId = _accountService.GetLoggedUserId();
            var uploadedImagesUrls = dto.UploadedImagesUrls;
            Product product = new Product()
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                ConditionId = dto.SelectedProductCondition,
                ProductCategoryId = dto.SelectedProductCategory,
                IsActive = true,
                DateCreated = DateTime.Now,
                ProductImages = uploadedImagesUrls.Select(imageUrl => new ProductImage()
                {
                    DateCreated = DateTime.Now,
                    ImagePath = imageUrl,
                    IsActive = true
                }).ToList()
            };
            await _databaseContext.Products.AddAsync(product);

            Offer offer = new Offer()
            {
                Product = product,
                IsActive = true,
                DateCreated = DateTime.Now,
                Price = dto.Price,
                SellerId = userId,
                StockQuantity = dto.StockQuantity,
                IsOfferPrivate = dto.IsOfferPrivate,
            };
            await _databaseContext.Offers.AddAsync(offer);

            //adding one selected parcel locker, it is optional
            if (dto.SelectedParcelLocker.HasValue)
            {
                await _databaseContext.OfferDeliveryTypes.AddAsync(new OfferDeliveryType()
                {
                    DeliveryTypeId = dto.SelectedParcelLocker.Value,
                    DateCreated = DateTime.Now,
                    Offer = offer,
                    IsActive = true,
                });
            }

            foreach (var deliveryId in dto.SelectedOtherDeliveries)
            {
                await _databaseContext.OfferDeliveryTypes.AddAsync(new OfferDeliveryType()
                {
                    DeliveryTypeId = deliveryId,
                    Offer = offer,
                    IsActive = true,
                    DateCreated = DateTime.Now
                });
            }

            await _databaseContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<UserOffersViewModel>> GetUserOffers()
        {
            Guid userId = _accountService.GetLoggedUserId();
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.SellerId == userId)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .Select(item => new UserOffersViewModel()
                {
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    DateEdited = item.DateEdited,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    ProductCategory = item.Product.ProductCategory.Name,
                    ProductStatus = item.IsOfferPrivate,
                    ProductName = item.Product.ProductName,
                    ImageUrl = item.Product.ProductImages.First().ImagePath
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetProductConditions()
        {
            return await _databaseContext.Conditions
              .Where(item => item.IsActive)
              .Select(item => new SelectListItem { Text = item.ConditionTitle, Value = item.Id.ToString() })
              .ToListAsync();
        }
        public async Task<List<SelectListItem>> GetProductCategories()
        {
            return await _databaseContext.ProductCategories
                .Where(item => item.IsActive)
                .Select(item => new SelectListItem { Text = item.Name, Value = item.Id.ToString() })
                .ToListAsync();
        }
        public async Task<List<DeliveryType>> GetDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes
                .Where(item => item.IsActive)
                .ToListAsync();
        }
        public async Task<List<DeliveryType>> GetParcelLockerDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes
                .Where(item => item.IsActive)
                .Where(item => item.Title.Contains("Locker"))
                .ToListAsync();
        }
        public async Task<List<SelectListItem>> GetOtherDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes
                .Where(item => item.IsActive)
                .Where(item => !item.Title.Contains("Locker"))
                .Select(item => new SelectListItem { Text = item.Title, Value = item.Id.ToString() })
                .ToListAsync();
        }
        public Task Edit(AddOfferViewModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<SingleOfferViewModel> GetOffer(int id)
        {
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.Id == id)
                .Include(item => item.Product)
                .Include(item => item.Seller)
                .Include(item => item.OfferDeliveryTypes)
                    .ThenInclude(item => item.DeliveryType)
                .Select(item => new SingleOfferViewModel()
                {
                    Condition = item.Product.Condition.ConditionTitle,
                    DateCreated = item.DateCreated.Date,
                    Seller = item.Seller.UserName!,
                    Description = item.Product.Description,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    Title = item.Product.ProductName,
                    isSellerCompany = (item.Seller.NIP.IsNullOrEmpty()) ? false : true,
                    Place = item.Seller.Address.Place,
                    PostalCity = item.Seller.Address.PostalCity,
                    PostalCode = item.Seller.Address.PostalCode,
                    ProductImages = item.Product.ProductImages
                        .Select(item => item.ImagePath)
                        .ToList(),
                    AvaliableDeliveryTypes = item.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeViewModel()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
                })
                .FirstAsync();
        }

        public async Task DeleteOffer(int id)
        {
            var offer = await _databaseContext.Offers.Where(item => item.Id == id)
                .FirstAsync();

            offer.IsActive = false;
            offer.DateDeleted = DateTime.Now;

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OfferBrowserViewModel>> GetAllOffers()
        {
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => !item.IsOfferPrivate)
                .Include(item => item.Seller)
                .Include(item => item.Product)
                .Select(item => new OfferBrowserViewModel()
                {
                    Id = item.Id,
                    Title = item.Product.ProductName,
                    Category = item.Product.ProductCategory.Name,
                    Condition = item.Product.Condition.ConditionTitle,
                    DateCreated = item.DateCreated.Date,
                    Price = item.Price,
                    SellerName = item.Seller.UserName!,
                    Description = item.Product.Description,
                    QuantityAvailable = item.StockQuantity,
                    ImageUrl = item.Product.ProductImages.First().ImagePath,
                }).ToListAsync();
        }

        public async Task<IEnumerable<MainPageCardViewModel>> GetIndexPageOffers()
        {
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => !item.IsOfferPrivate)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .OrderByDescending(item => item.DateCreated)
                .Take(6)
                .Select(item => new MainPageCardViewModel()
                {
                    Id = item.Id,
                    Price = item.Price,
                    Title = item.Product.ProductName,
                    ImagePath = item.Product.ProductImages.First().ImagePath,
                }).ToListAsync();
        }
    }
}
