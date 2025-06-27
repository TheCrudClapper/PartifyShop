using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
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
        public async Task<List<SelectListItemDto>> GetOfferPictures(int offerId)
        {
            var items = await _productImageRepo.GetImagesFromOfferAsync(offerId);

            return items.Select(path => new SelectListItemDto
            {
                Value = path.ImagePath,
                Text = path.ImagePath,
            })
           .ToList();
        }
    }
}
