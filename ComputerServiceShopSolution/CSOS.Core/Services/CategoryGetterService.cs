using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Universal;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class CategoryGetterService : ICategoryGetterService
    {
        public readonly IProductCategoryRepository _productCategoryRepo;
        public CategoryGetterService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepo = productCategoryRepository;
        }

        public async Task<IEnumerable<MainPageCardResponse>> GetProductCategoriesAsMainPageCardResponseDto()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => new MainPageCardResponse()
            {
                Id = item.Id,
                ImageUrl = item.CategoryImage,
                Title = item.Name,
            });
        }
        public async Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => item.ToSelectListItem());
        }
    }
}
