using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.Models.Contexts;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ComputerServiceOnlineShop.Services
{
    public class OfferService : IOfferService
    {
        private readonly DatabaseContext _databaseContext;
        public OfferService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task Add(OfferViewModel model)
        {
            ////user id for testing purposes
            //var uploadedImagesUrls = model.UploadedImagesUrls;
            //Product product = new Product()
            //{
            //    ProductName = model.ProductName,
            //    Description = model.Description,
            //    ConditionId = int.Parse(model.SelectedProductCondition),
            //    ProductCategoryId = int.Parse(model.SelectedProductCategory),
            //    IsActive = true,
            //    DateCreated = DateTime.Now,
            //    ProductImages = uploadedImagesUrls.Select(imageUrl => new ProductImage()
            //    {
            //        DateCreated = DateTime.Now,
            //        ImagePath = imageUrl,
            //        IsActive = true
            //    }).ToList()
            //};
            //await _databaseContext.Products.AddAsync(product);

            //Offer offer = new Offer()
            //{
            //    Product = product,
            //    IsActive = true,
            //    DateCreated = DateTime.Now,
            //    Price = model.Price,
            //    SellerId = userId,
            //    StockQuantity = model.StockQuantity,
            //    OfferStatus = true,
            //};
            //await _databaseContext.Offers.AddAsync(offer);

            ////adding one selected parcel locker, it is optional
            //if (model.SelectedParcelLocker.HasValue)
            //{
            //    await _databaseContext.OfferDeliveryTypes.AddAsync(new OfferDeliveryType()
            //    {
            //        DeliveryTypeId = model.SelectedParcelLocker.Value,
            //        DateCreated = DateTime.Now,
            //        Offer = offer,
            //        IsActive = true,
            //    });
            //}

            //foreach (var deliveryId in model.SelectedOtherDeliveries)
            //{
            //    await _databaseContext.OfferDeliveryTypes.AddAsync(new OfferDeliveryType()
            //    {
            //        DeliveryTypeId = deliveryId,
            //        Offer = offer,
            //        IsActive = true,
            //        DateCreated = DateTime.Now
            //    });
            //}

            //await _databaseContext.SaveChangesAsync();
        }
        public  async Task<List<UserOffersViewModel>> GetUserOffers()
        {
            return await _databaseContext.Offers.Where(item => item.IsActive == true)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .Select(item => new UserOffersViewModel()
                {
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    DateEdited = item.DateEdited,
                    Price = item.Price,
                    ProductCategory = item.Product.ProductCategory.Name,
                    ProductStatus = item.OfferStatus,
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
    }
}
