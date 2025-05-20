using ComputerServiceOnlineShop.Entities.Contexts;
using CSOS.Core.DTO;
using CSOS.Core.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Core.Services
{
    public class ConditionGetterService : IConditionGetterService
    {

        public readonly DatabaseContext _databaseContext;
        public ConditionGetterService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<SelectListItemDto>> GetProductCategoriesAsSelectList()
        {
            return await _databaseContext.ProductCategories
                .Where(item => item.IsActive)
                .Select(item => new SelectListItemDto { Text = item.Name, Value = item.Id.ToString() })
                .ToListAsync();
        }

        public async Task<List<SelectListItemDto>> GetProductConditions()
        {
            return await _databaseContext.Conditions
              .Where(item => item.IsActive)
              .Select(item => new SelectListItemDto { Text = item.ConditionTitle, Value = item.Id.ToString() })
              .ToListAsync();
        }

    }
}
