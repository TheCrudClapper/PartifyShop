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
        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepo = productImageRepository;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetOfferPictures(int offerId)
        {
            var items = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            return items.Select(item => item.ToSelectListItem()).ToList();
        }

        //change this to take productiamges instead of downloading once again offer
        public async Task<Result> DeleteImagesFromOffer(int offerId, List<string> imageUrls)
        {
            IEnumerable<ProductImage> productImages = await _productImageRepo.GetImagesFromOfferAsync(offerId);
            if (productImages.Count() == 0)
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
