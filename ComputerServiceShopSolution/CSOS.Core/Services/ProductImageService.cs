using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepo;
        private readonly IUnitOfWork _unitOfWork;
        public ProductImageService(IProductImageRepository productImageRepository, IUnitOfWork unitOfWork)
        {
            _productImageRepo = productImageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemDto>> GetOfferPictures(int offerId)
        {
            var items = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            return items.Select(item => item.ToSelectListItem()).ToList();
        }

        public async Task<Result> DeleteImagesFromOffer(int offerId, List<string> imageUrls)
        {
            List<ProductImage>? productImages = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            if (productImages == null)
                return Result.Failure(OfferErrors.OfferDoesNotExist);

            if (productImages.Count == 0)
                return Result.Failure(ProductImageErrors.ProductImagesAreEmpty);

            foreach (var image in productImages)
            {
                if (imageUrls.Any(url => image.ImagePath.Contains(url)))
                {
                    image.DateDeleted = DateTime.UtcNow;
                    image.IsActive = false;
                }
            }

            return Result.Success();
        }
    }
}
