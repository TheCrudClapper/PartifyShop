using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
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

        public async Task<List<MainPageCardResponseDto>> GetProductCategoriesAsMainPageCardResponseDto()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => new MainPageCardResponseDto()
            {
                Id = item.Id,
                ImagePath = item.CategoryImage,
                Title = item.Name,
            })
           .ToList();
        }
        public async Task<List<SelectListItemDto>> GetProductCategoriesAsSelectList()
        {
            var categories = await _productCategoryRepo.GetAllProductCategoriesAsync();

            return categories.Select(item => new SelectListItemDto()
            {
                Text = item.Name,
                Value = item.Id + "",
            })
            .ToList();
        }
    }
}
