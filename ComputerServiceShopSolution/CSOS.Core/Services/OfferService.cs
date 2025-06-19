using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Exceptions;
using CSOS.Core.Helpers;
using CSOS.Core.Mappings.ToEntity.OfferDeliveryTypeMappings;
using CSOS.Core.Mappings.ToEntity.OfferMappings;
using CSOS.Core.Mappings.ToEntity.ProductMappings;
using CSOS.Core.ServiceContracts;
namespace ComputerServiceOnlineShop.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepo;
        private readonly IProductRepository _productRepo;
        private readonly IOfferDeliveryTypeRepository _offerDeliveryTypeRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly IDeliveryTypeGetterService _deliveryTypeGetterService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public OfferService(
            IAccountService accountService,
            IDeliveryTypeGetterService deliveryTypeGetterService,
            IUnitOfWork unitOfWork,
            IOfferRepository offerRepo,
            IProductRepository productRepo,
            IOfferDeliveryTypeRepository offerDeliveryTypeRepo,
            IProductImageRepository productImageRepo,
            ICurrentUserService currentUserService
            )
        {
            _productRepo = productRepo;
            _offerDeliveryTypeRepo = offerDeliveryTypeRepo;
            _offerRepo = offerRepo;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _deliveryTypeGetterService = deliveryTypeGetterService;
            _productImageRepo = productImageRepo;
        }
        public async Task Add(AddOfferDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Guid userId = _currentUserService.GetUserId();
            var uploadedImagesUrls = dto.UploadedImagesUrls;

            Product product = dto.ToProductEntity();
            await _productRepo.AddAsync(product);

            Offer offer = dto.ToOfferEntity(product, userId);
            await _offerRepo.AddAsync(offer);

            //adding one selected parcel locker, it is optional
            if (dto.SelectedParcelLocker.HasValue)
            {
                OfferDeliveryType parcelLockerDelivery = dto.ToOfferDeliveryTypeEntity(offer);
                await _offerDeliveryTypeRepo.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToOfferDeliveryTypeEntity(offer)).ToList();
            await _offerDeliveryTypeRepo.AddRangeAsync(deliveryTypes);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task Edit(int id, EditOfferDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithDetailsToEdit(id, userId);

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
            if (dto.ImagesToDelete?.Count > 0 && dto.ImagesToDelete != null)
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
                OfferDeliveryType parcelLockerDelivery = dto.ToOfferDeliveryTypeEntity(offer);
                await _offerDeliveryTypeRepo.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToOfferDeliveryTypeEntity(offer)).ToList();
            await _offerDeliveryTypeRepo.AddRangeAsync(deliveryTypes);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteOffer(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetUserOffersByIdAsync(id, userId);

            if (offer == null)
                throw new EntityNotFoundException("Entity not found or you are not the owner");

            _offerRepo.SoftDelete(offer);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserOffersResponseDto>> GetFilteredUserOffers(string? title)
        {
            Guid userId = _currentUserService.GetUserId();
            var offers = await _offerRepo.GetFilteredUserOffersAsync(title, userId);

            var items = offers.Select(item => new UserOffersResponseDto()
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
            .ToList();
            return items;

        }

        public async Task<EditOfferResponseDto> GetOfferForEdit(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithAllDetailsByUserAsync(id, userId);
            if (offer == null)
                throw new EntityNotFoundException("This offer doesn't exist or doesn't belong to you");

            return new EditOfferResponseDto()
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                Price = offer.Price,
                ExistingImagesUrls = offer.Product.ProductImages
                    .Where(item => item.IsActive)
                    .Select(item => new SelectListItemDto()
                    {
                        Value = item.ImagePath,
                        Text = item.ImagePath,
                    })
                    .ToList(),
                Description = offer.Product.Description,
                SelectedProductCondition = offer.Product.Condition.Id.ToString(),
                SelectedProductCategory = offer.Product.ProductCategory.Id.ToString(),
                IsOfferPrivate = offer.IsOfferPrivate,
                StockQuantity = offer.StockQuantity,
                SelectedOtherDeliveries = offer.OfferDeliveryTypes
                    .Where(item => !item.DeliveryType.Title.Contains("Locker"))
                    .Select(item => item.DeliveryTypeId)
                    .ToList(),
                SelectedParcelLocker = offer.OfferDeliveryTypes
                    .Where(item => item.DeliveryType.Title.Contains("Locker"))
                    .Select(item => item.DeliveryTypeId)
                    .FirstOrDefault(),
            };
        }

        public async Task<List<SelectListItemDto>> GetOfferPictures(int id)
        {
            return (await _offerRepo.GetOfferPicturesAsSelectListDto(id)).ToList();
        }

        //OK
        public async Task<OfferBrowserResponseDto> GetFilteredOffers(OfferFilter filter)
        {
            var offers = await _offerRepo.GetFilteredOffersAsync(filter);
            var items = offers.Select(item => new OfferBrowserItemResponseDto()
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
            })
            .ToList();

            return new OfferBrowserResponseDto()
            {
                Items = items,
                Filter = filter,
                SortingOptions = GetSortingOptions(),
                DeliveryOptions = await _deliveryTypeGetterService.GetAllDeliveryTypes(),
            };
        }
        //OK
        public async Task<List<MainPageCardResponseDto>> GetIndexPageOffers()
        {
            var offers = await _offerRepo.GetOffersByTakeAsync();
            return offers.Select(item => new MainPageCardResponseDto()
            {
                Id = item.Id,
                ImagePath = item.Product.ProductImages.First().ImagePath,
                Price = item.Price,
                Title = item.Product.ProductName,
            })
            .ToList();
        }
        //OK
        public async Task<bool> DoesOfferExist(int id)
        {
            return await _offerRepo.IsOfferInDb(id);
        }
        public async Task DeleteImagesFromOffer(int offerId, List<string> imageUrls)
        {
            var productImages = await _productImageRepo.GetImagesFromOffer(offerId, imageUrls);

            foreach (var image in productImages)
            {
                image.IsActive = false;
                image.DateDeleted = DateTime.Now;
            }

            await _unitOfWork.SaveChangesAsync();
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
            var count = await _offerRepo.GetNonPrivateOfferCount();
            if (count == 0)
                return new();

            var take = Math.Min(count, 7);

            var offers = await _offerRepo.GetOffersByTakeAsync(take);
            return offers.Select(item => new MainPageCardResponseDto()
            {
                Id = item.Id,
                ImagePath = item.Product.ProductImages.FirstOrDefault()?.ImagePath ?? "sex",
                Price = item.Price,
                Title = item.Product.ProductName,
            })
            .ToList();

        }

        public async Task<OfferResponseDto> GetOffer(int id)
        {
            var offer = await _offerRepo.GetOfferWithAllDetailsAsync(id);
            if (offer == null)
                throw new EntityNotFoundException("Item of given ID not found");

            var dto = new OfferResponseDto()
            {
                Id = offer.Id,
                Condition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated.Date,
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                Price = offer.Price,
                StockQuantity = offer.StockQuantity,
                Title = offer.Product.ProductName,
                Place = offer.Seller.Address.Place,
                PostalCity = offer.Seller.Address.PostalCity,
                PostalCode = offer.Seller.Address.PostalCode,
                ProductImages = offer.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
                AvaliableDeliveryTypes = offer.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponseDto()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
            };

            return dto;
        }
    }
}
