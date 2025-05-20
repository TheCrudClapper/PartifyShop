using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Exceptions;
using CSOS.Core.Helpers;
using CSOS.Core.Mappings.OfferDeliveryTypeMappings;
using CSOS.Core.Mappings.OfferMappings;
using CSOS.Core.Mappings.ProductMappings;
using CSOS.Core.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
namespace ComputerServiceOnlineShop.Services
{
    public class OfferService : IOfferService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IOfferRepository _offerRepo;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        public OfferService(DatabaseContext databaseContext,IAccountService accountService, IDeliveryTypeGetterService deliveryTypeGetterService, IUnitOfWork unitOfWork, IOfferRepository offerRepo)
        {
            _offerRepo = offerRepo;
            _databaseContext = databaseContext;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _deliveryTypeGetterService = deliveryTypeGetterService;
        }
        public async Task Add(AddOfferDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Guid userId = _accountService.GetLoggedUserId();
            var uploadedImagesUrls = dto.UploadedImagesUrls;

            Product product = dto.ToEntity();
            await _databaseContext.Products.AddAsync(product);

            Offer offer = dto.ToEntity(product, userId);
            await _offerRepo.AddAsync(offer);
            await _databaseContext.Offers.AddAsync(offer);

            //adding one selected parcel locker, it is optional
            if (dto.SelectedParcelLocker.HasValue)
            {
                OfferDeliveryType parcelLockerDelivery = dto.ToEntity(offer);
                await _databaseContext.OfferDeliveryTypes.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToEntity(offer)).ToList();
            await _databaseContext.OfferDeliveryTypes.AddRangeAsync(deliveryTypes);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task Edit(int id, EditOfferDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            var offer = await _databaseContext.Offers
                .Where(item => item.Id == id && item.IsActive)
                .Include(item => item.Product)
                    .ThenInclude(item => item.ProductImages)
                .Include(item => item.OfferDeliveryTypes)
                    .ThenInclude(item => item.DeliveryType)
                .FirstOrDefaultAsync();

            if (offer == null)
                throw new EntityNotFoundException("Entity of given id was not found");

            offer.IsOfferPrivate = dto.IsOfferPrivate;
            offer.StockQuantity = dto.StockQuantity;
            offer.Price = dto.Price;

            var product = offer.Product;
            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.ConditionId = dto.SelectedProductCondition;
            product.ProductCategoryId = dto.SelectedProductCategory;

            //deletes images checked by user
            if(dto.ImagesToDelete?.Count > 0 && dto.ImagesToDelete != null)
            {
                await DeleteImagesFromOffer(id, dto.ImagesToDelete);
            }

            //saving new images if any
            if (dto.UploadedImagesUrls != null && dto.UploadedImagesUrls.Count > 0)
            {
                foreach (var url in dto.UploadedImagesUrls)
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ImagePath = url,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    });
                }
            }

            //clear all existing delivery types
            offer.OfferDeliveryTypes.Clear();

            if (dto.SelectedParcelLocker.HasValue)
            {
                OfferDeliveryType parcelLockerDelivery = dto.ToEntity(offer);
                await _databaseContext.OfferDeliveryTypes.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToEntity(offer)).ToList();
            await _databaseContext.OfferDeliveryTypes.AddRangeAsync(deliveryTypes);

            await _databaseContext.SaveChangesAsync();
        }
        public async Task DeleteOffer(int id)
        {
            var userId = _accountService.GetLoggedUserId();
            var offer = await _databaseContext.Offers.Where(item => item.Id == id && item.SellerId == userId)
                .FirstOrDefaultAsync();

            if (offer == null)
                throw new EntityNotFoundException("Entity not found or you are not the owner");

            offer.IsActive = false;
            offer.DateDeleted = DateTime.Now;

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<UserOffersResponseDto>> GetUserOffers()
        {
            Guid userId = _accountService.GetLoggedUserId();
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.SellerId == userId)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .Select(item => new UserOffersResponseDto()
                {
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    ProductCategory = item.Product.ProductCategory.Name,
                    ProductStatus = item.IsOfferPrivate,
                    ProductName = item.Product.ProductName,
                    ImageUrl = item.Product.ProductImages
                        .Where(item => item.IsActive)
                        .First().ImagePath
                })
                .ToListAsync();
        }
        public async Task<List<UserOffersResponseDto>> GetFilteredUserOffers(string? title)
        {
            Guid userId = _accountService.GetLoggedUserId();

            var query =  _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.SellerId == userId)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(title))
                    query = query.Where(item => item.Product.ProductName.Contains(title));

                var items = await query.Select(item => new UserOffersResponseDto()
                {
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    ProductCategory = item.Product.ProductCategory.Name,
                    ProductStatus = item.IsOfferPrivate,
                    ProductName = item.Product.ProductName,
                    ImageUrl = item.Product.ProductImages
                        .Where(item => item.IsActive)
                        .First().ImagePath
                })
                .ToListAsync();

            return items;
        }
        public async Task<SingleOfferResponseDto> GetOffer(int id)
        {
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.Id == id)
                .Include(item => item.Product)
                .Include(item => item.Seller)
                .Include(item => item.OfferDeliveryTypes)
                    .ThenInclude(item => item.DeliveryType)
                .Select(item => new SingleOfferResponseDto()
                {
                    Id = item.Id,
                    Condition = item.Product.Condition.ConditionTitle,
                    DateCreated = item.DateCreated.Date,
                    Seller = item.Seller.UserName!,
                    Description = item.Product.Description,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    Title = item.Product.ProductName,
                    isSellerCompany = (string.IsNullOrEmpty(item.Seller.NIP)) ? false : true,
                    Place = item.Seller.Address.Place,
                    PostalCity = item.Seller.Address.PostalCity,
                    PostalCode = item.Seller.Address.PostalCode,
                    ProductImages = item.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
                    AvaliableDeliveryTypes = item.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponseDto()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
                })
                .FirstAsync();
        }
        public async Task<EditOfferResponseDto> GetOfferForEdit(int id)
        {
            Guid userId = _accountService.GetLoggedUserId();
            return await _databaseContext.Offers
                .Where(item => item.IsActive)
                .Where(item => item.SellerId == userId)
                .Where(item => item.Id == id)
                .Include(item => item.Product)
                .Select(item => new EditOfferResponseDto()
                {
                    Id = item.Id,
                    ProductName = item.Product.ProductName,
                    Price = item.Price,
                    ExistingImagesUrls = item.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => new SelectListItemDto()
                        {
                            Value = item.ImagePath,
                            Text = item.ImagePath,
                        })
                        .ToList(),
                    Description = item.Product.Description,
                    SelectedProductCondition = item.Product.Condition.Id.ToString(),
                    SelectedProductCategory = item.Product.ProductCategory.Id.ToString(),
                    IsOfferPrivate = item.IsOfferPrivate,
                    StockQuantity = item.StockQuantity,
                    SelectedOtherDeliveries = item.OfferDeliveryTypes
                        .Where(item => !item.DeliveryType.Title.Contains("Locker"))
                        .Select(item => item.DeliveryTypeId)
                        .ToList(),
                    SelectedParcelLocker = item.OfferDeliveryTypes
                        .Where(item => item.DeliveryType.Title.Contains("Locker"))
                        .Select(item => item.DeliveryTypeId)
                        .FirstOrDefault(),
                })
                .FirstAsync();
        }
        public async Task<List<SelectListItemDto>> GetOfferPictures(int id)
        {
            var imagePaths = await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => item.Id == id)
                .Include(item => item.Product)
                    .ThenInclude(item => item.ProductImages)
                .SelectMany(item => item.Product.ProductImages
                    .Where(img => img.IsActive)
                    .Select(img => img.ImagePath))
                .ToListAsync();

            var imageSelectList = imagePaths.Select(path => new SelectListItemDto
            {
                Value = path,
                Text = path,
            }).ToList();

            return imageSelectList;
        }
        
        public async Task<OfferBrowserResponseDto> GetFilteredOffers(OfferFilter filter)
        {
            var query = _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => !item.IsOfferPrivate)
                .Include(item => item.Seller)
                .Include(item => item.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
                query = query.Where(item => item.Product.ProductName.Contains(filter.SearchPhrase) || item.Product.Description.Contains(filter.SearchPhrase));

            if (filter.CategoryId != null)
                query = query.Where(item => item.Product.ProductCategoryId == filter.CategoryId);

            if (filter.PriceFrom.HasValue)
                query = query.Where(item => item.Price >= filter.PriceFrom);

            if (filter.PriceTo.HasValue)
                query = query.Where(item => item.Price <= filter.PriceTo);

            if (filter.SortOption == "price_desc")
                query = query.OrderByDescending(item => item.Price);

            if (filter.SortOption == "price_asc")
                query = query.OrderBy(item => item.Price);


            if (!string.IsNullOrWhiteSpace(filter.DeliveryOption))
            {
                if (int.TryParse(filter.DeliveryOption, out int deliveryId))
                {
                    query = query.Where(item =>
                        item.OfferDeliveryTypes.Any(dt => dt.DeliveryTypeId == deliveryId));
                }
            }
            
            var items = await query.Select(item => new OfferBrowserItemResponseDto()
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

            return new OfferBrowserResponseDto()
            {
                Items = items,
                Filter = filter,
                SortingOptions = GetSortingOptions(),
                DeliveryOptions = await _deliveryTypeGetterService.GetAllDeliveryTypes(),
            };
        }
        public async Task<IEnumerable<MainPageCardResponseDto>> GetIndexPageOffers()
        {
            return await _databaseContext.Offers.Where(item => item.IsActive)
                .Where(item => !item.IsOfferPrivate)
                .Include(item => item.Product)
                .ThenInclude(item => item.ProductImages)
                .OrderByDescending(item => item.DateCreated)
                .Take(12)
                .Select(item => new MainPageCardResponseDto()
                {
                    Id = item.Id,
                    Price = item.Price,
                    Title = item.Product.ProductName,
                    ImagePath = item.Product.ProductImages
                    .Where(item => item.IsActive).
                    First().ImagePath,
                })
                .ToListAsync();
        }
        public async Task<bool> DoesOfferExist(int id)
        {
            Guid loggedUserId = _accountService.GetLoggedUserId();
            return await _databaseContext.Offers
                .AnyAsync(item => item.Id == id && item.SellerId == loggedUserId && item.IsActive);
        }
        public async Task DeleteImagesFromOffer(int offerId, List<string> imageUrls)
        {
            var productImages = await _databaseContext.Offers
                .Where(item => item.IsActive && item.Id == offerId)
                .Include(item => item.Product)
                    .ThenInclude(item => item.ProductImages)
                    .SelectMany(item => item.Product.ProductImages)
                    .Where(item => imageUrls.Contains(item.ImagePath) && item.IsActive)
                    .ToListAsync();

            foreach (var image in productImages)
            {
                image.IsActive = false;
                image.DateDeleted = DateTime.Now;
            }
            await _databaseContext.SaveChangesAsync();
        }

        //Just for testing
        //Later create table in db for filtering options
        public List<SelectListItemDto> GetSortingOptions()
        {
            return new List<SelectListItemDto>()
            {
                new SelectListItemDto()
                {
                    Text = "Price - from highest",
                    Value = "price_desc",
                },
                new SelectListItemDto()
                {
                    Text = "Price - from lowest",
                    Value = "price_asc",
                }
            };
        }
        public async Task<List<MainPageCardResponseDto>> GetDealsOfTheDay()
        {
            //add daily reshuffle logic
            var count = await _databaseContext.Offers.CountAsync(item => item.IsActive && !item.IsOfferPrivate);
            if (count == 0)
                return new();

            var take = Math.Min(count, 7);

            return await _databaseContext.Offers.Where(item => item.IsActive && !item.IsOfferPrivate)
                .OrderBy(item => Guid.NewGuid())
                .Take(take)
                .Select(item => new MainPageCardResponseDto
                {
                    Id = item.Id,
                    ImagePath = item.Product.ProductImages
                        .Where(item => item.IsActive)
                        .First().ImagePath,
                    Price = item.Price,
                    Title = item.Product.ProductName,

                }).ToListAsync();
        }
    }
}
