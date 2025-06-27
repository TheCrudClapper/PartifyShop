using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.ExternalServicesContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Helpers;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.Mappings.ToEntity.OfferDeliveryTypeMappings;
using CSOS.Core.Mappings.ToEntity.OfferMappings;
using CSOS.Core.Mappings.ToEntity.ProductMappings;
using CSOS.Core.ServiceContracts;

namespace ComputerServiceOnlineShop.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepo;
        private readonly IPictureHandlerService _pictureHandlerService;
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
            ICurrentUserService currentUserService,
            IPictureHandlerService pictureHandlerService
            )
        {
            _productRepo = productRepo;
            _offerDeliveryTypeRepo = offerDeliveryTypeRepo;
            _offerRepo = offerRepo;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _deliveryTypeGetterService = deliveryTypeGetterService;
            _productImageRepo = productImageRepo;
            _pictureHandlerService = pictureHandlerService;
        }
        public async Task<Result> Add(AddOfferDto dto)
        {
            if (dto == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();

            dto.UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(dto.UploadedImages);

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

            return Result.Success();
        }
        public async Task<Result> Edit(int id, EditOfferDto dto)
        {
            if (dto == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithDetailsToEditAsync(id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferNotFound);

            offer.IsOfferPrivate = dto.IsOfferPrivate;
            offer.StockQuantity = dto.StockQuantity;
            offer.Price = dto.Price;

            var product = offer.Product;
            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.ConditionId = dto.SelectedProductCondition;
            product.ProductCategoryId = dto.SelectedProductCategory;

            //deletes images checked by user
            if (dto.ImagesToDelete?.Count > 0)
            {
                var result = await DeleteImagesFromOffer(id, dto.ImagesToDelete);

                if (result.IsFailure)
                    return Result.Failure(result.Error);
            }

            dto.UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(dto.UploadedImages);


            //saving new images if any
            if (dto.UploadedImagesUrls.Count > 0)
            {
                foreach (var url in dto.UploadedImagesUrls)
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ImagePath = url,
                        IsActive = true,
                        DateCreated = DateTime.UtcNow
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

            return Result.Success();
        }

        public async Task<Result> DeleteOffer(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetUserOffersByIdAsync(id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferNotFound);

            offer.DateDeleted = DateTime.UtcNow;
            offer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<List<UserOffersResponseDto>> GetFilteredUserOffers(string? title)
        {
            Guid userId = _currentUserService.GetUserId();
            var offers = await _offerRepo.GetFilteredUserOffersAsync(title, userId);

            var items = offers.ToListUserOffersResponseDto();
            return items;
        }

        public async Task<Result<EditOfferResponseDto>> GetOfferForEdit(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithAllDetailsByUserAsync(id, userId);
            if (offer == null)
                return Result.Failure<EditOfferResponseDto>(OfferErrors.OfferNotFound);

            var dto = offer.ToEditOfferResponseDto();
            return dto;
        }

        public async Task<OfferBrowserResponseDto> GetFilteredOffers(OfferFilter filter)
        {
            var offers = await _offerRepo.GetFilteredOffersAsync(filter);

            var items = offers.ToListOfferItemBrowserResponseDto();

            return new OfferBrowserResponseDto()
            {
                Items = items,
                Filter = filter,
                SortingOptions = GetSortingOptions(),
                DeliveryOptions = await _deliveryTypeGetterService.GetAllDeliveryTypes(),
            };
        }

        public async Task<List<MainPageCardResponseDto>> GetIndexPageOffers()
        {
            var offers = await _offerRepo.GetOffersByTakeAsync();
            return offers.ToListMainPageCardDto();
        }

        public async Task<bool> DoesOfferExist(int id)
        {
            return await _offerRepo.IsOfferInDbAsync(id);
        }

        public async Task<Result> DeleteImagesFromOffer(int offerId, List<string> imageUrls)
        {
            List<ProductImage>? productImages = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            if (productImages == null)
                return Result.Failure(OfferErrors.OfferNotFound);

            if(productImages.Count == 0)
                return Result.Failure(ProductImageErrors.ProductImagesAreEmpty);

            foreach (var image in productImages)
            {
                if (imageUrls.Any(url => image.ImagePath.Contains(url)))
                {
                    image.DateDeleted = DateTime.UtcNow;
                    image.IsActive = false;
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
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

            return offers.ToListMainPageCardDto();
        }

        public async Task<Result<OfferResponseDto>> GetOffer(int id)
        {
            var offer = await _offerRepo.GetOfferWithAllDetailsAsync(id);

            if (offer == null)
                return Result.Failure<OfferResponseDto>(OfferErrors.OfferNotFound);

            var dto = offer.ToOfferResponseDto();

            return dto;
        }
    }
}
