using ComputerServiceOnlineShop.Entities.Contexts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Core.Services
{
    public class CategoryGetterService : ICategoryGetterService
    {
        public readonly DatabaseContext _databaseContext;
        public CategoryGetterService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<MainPageCardResponseDto>> GetProductCategories()
        {
            return await _databaseContext.ProductCategories.Where(item => item.IsActive)
                .Select(item => new MainPageCardResponseDto
                {
                    Id = item.Id,
                    ImagePath = item.CategoryImage,
                    Title = item.Name,
                }).ToListAsync();
        }
        public async Task<List<SelectListItemDto>> GetProductCategoriesAsSelectList()
        {
            return await _databaseContext.ProductCategories
                .Where(item => item.IsActive)
                .Select(item => new SelectListItemDto { Text = item.Name, Value = item.Id.ToString() })
                .ToListAsync();
        }
    }
}
