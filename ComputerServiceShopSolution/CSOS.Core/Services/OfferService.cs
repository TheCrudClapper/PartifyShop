using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.ExternalServicesContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.DtoContracts;
using CSOS.Core.DTO.Requests;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Helpers;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.Mappings.ToEntity.OfferDeliveryTypeMappings;
using CSOS.Core.Mappings.ToEntity.OfferMappings;
using CSOS.Core.Mappings.ToEntity.ProductMappings;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepo;
        private readonly ISortingOptionService _sortingOptionService;
        private readonly IPictureHandlerService _pictureHandlerService;
        private readonly IProductImageService _productImageService;
        private readonly IProductRepository _productRepo;
        private readonly IOfferDeliveryTypeRepository _offerDeliveryTypeRepo;
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
            ICurrentUserService currentUserService,
            IPictureHandlerService pictureHandlerService,
            IProductImageService productImageService,
            ISortingOptionService sortingOptionService
            )
        {
            _productRepo = productRepo;
            _offerDeliveryTypeRepo = offerDeliveryTypeRepo;
            _offerRepo = offerRepo;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _deliveryTypeGetterService = deliveryTypeGetterService;
            _pictureHandlerService = pictureHandlerService;
            _productImageService = productImageService;
            _sortingOptionService = sortingOptionService;
        }
        public async Task<Result> Add(OfferAddRequest? addRequest)
        {
            if (addRequest == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();

            Offer offer = addRequest.ToOfferEntity(userId);
            await _offerRepo.AddAsync(offer);

            Product product = addRequest.ToProductEntity(offer);
            await _productRepo.AddAsync(product);

            await SaveNewImagesAsync(addRequest, product);
            await AddDeliveryTypesAsync(addRequest, offer);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result> Edit(OfferUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
                return Result.Failure(OfferErrors.OfferIsNull);

            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithDetailsToEditAsync(updateRequest.Id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            offer.IsOfferPrivate = updateRequest.IsOfferPrivate;
            offer.StockQuantity = updateRequest.StockQuantity;
            offer.Price = updateRequest.Price;

            Product product = offer.Product;
            product.ProductName = updateRequest.ProductName;
            product.Description = updateRequest.Description;
            product.ConditionId = updateRequest.SelectedProductCondition;
            product.ProductCategoryId = updateRequest.SelectedProductCategory;

            //deletes images checked by user
            if (updateRequest.ImagesToDelete?.Count > 0)
            {
                var result =  _productImageService.DeleteImagesFromOffer(product.ProductImages, updateRequest.ImagesToDelete);

                if (result.IsFailure)
                    return Result.Failure(result.Error);
            }

            await SaveNewImagesAsync(updateRequest, product);

            //clean existing deliveries for offer
            offer.OfferDeliveryTypes.Clear();

            //add new ones
            await AddDeliveryTypesAsync(updateRequest, offer);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteOffer(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetUserOffersByIdAsync(id, userId);

            if (offer == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            offer.DateDeleted = DateTime.UtcNow;
            offer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<IEnumerable<UserOffersResponseDto>> GetFilteredUserOffers(string? title)
        {
            Guid userId = _currentUserService.GetUserId();
            var offers = await _offerRepo.GetFilteredUserOffersAsync(title, userId);

            var items = offers.ToIEnumerableUserOffersResponseDto();
            return items;
        }

        public async Task<Result<EditOfferResponseDto>> GetOfferForEdit(int id)
        {
            Guid userId = _currentUserService.GetUserId();
            var offer = await _offerRepo.GetOfferWithAllDetailsByUserAsync(id, userId);
            if (offer == null)
                return Result.Failure<EditOfferResponseDto>(OfferErrors.OfferDoesNotExist);

            var dto = offer.ToEditOfferResponseDto();
            return dto;
        }

        public async Task<OfferBrowserResponseDto> GetFilteredOffers(OfferFilter filter)
        {
            var offers = await _offerRepo.GetFilteredOffersAsync(filter);

            var items = offers.ToListOfferItemBrowserResponseDto();

            return new OfferBrowserResponseDto()
            {
                Items = items.ToList(),
                Filter = filter,
                SortingOptions = _sortingOptionService.GetSortingOptions().ToList(),
                DeliveryOptions = (await _deliveryTypeGetterService.GetAllDeliveryTypesAsSelectionList()).ToList(),
            };
        }

        public async Task<IEnumerable<MainPageCardResponseDto>> GetIndexPageOffers()
        {
            var offers = await _offerRepo.GetOffersByTakeAsync();
            return offers.ToIEnumerableMainPageCardDto();
        }

        public async Task<bool> DoesOfferExist(int id)
        {
            return await _offerRepo.IsOfferInDbAsync(id);
        }

        public async Task<IEnumerable<MainPageCardResponseDto>> GetDealsOfTheDay()
        {
            //add daily reshuffle logic
            var count = await _offerRepo.GetNonPrivateOfferCount();
            if (count == 0)
                return Enumerable.Empty<MainPageCardResponseDto>();

            var take = Math.Min(count, 7);

            var offers = await _offerRepo.GetOffersByTakeAsync(take);

            return offers.ToIEnumerableMainPageCardDto();
        }

        public async Task<Result<OfferResponseDto>> GetOffer(int id)
        {
            var offer = await _offerRepo.GetOfferWithAllDetailsAsync(id);

            if (offer == null || offer.IsOfferPrivate)
                return Result.Failure<OfferResponseDto>(OfferErrors.OfferDoesNotExist);

            var dto = offer.ToOfferResponseDto();

            return dto;
        }

        private async Task SaveNewImagesAsync(IOfferImageDto dto, Product product)
        {
            dto.UploadedImagesUrls = await _pictureHandlerService.SavePicturesToDirectory(dto.UploadedImages);

            if (dto.UploadedImagesUrls?.Count > 0)
            {
                var newImages = dto.UploadedImagesUrls.Select(url => new ProductImage
                {
                    ImagePath = url,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow
                });

                foreach (var image in newImages)
                {
                    product.ProductImages.Add(image); 
                }
            }
        }
        private async Task AddDeliveryTypesAsync(IOfferDeliveryDto dto, Offer offer)
        {
            if (dto.SelectedParcelLocker.HasValue)
            {
                var parcelLockerDelivery = dto.ToOfferDeliveryTypeEntity(offer);
                await _offerDeliveryTypeRepo.AddAsync(parcelLockerDelivery);
            }

            var deliveryTypes = dto.SelectedOtherDeliveries
                .Select(deliveryId => deliveryId.ToOfferDeliveryTypeEntity(offer));

            await _offerDeliveryTypeRepo.AddRangeAsync(deliveryTypes);
        }
    }
}
