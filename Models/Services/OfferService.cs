using ComputerServiceOnlineShop.Models.Contexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace ComputerServiceOnlineShop.Models.Services
{
    public class OfferService : BaseService
    {
        public OfferService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
        public async Task<List<SelectListItem>> GetProductConditions()
        {
            return (await DatabaseContext.Conditions.ToListAsync())
              .Select(item => new SelectListItem { Text = item.ConditionTitle, Value = item.Id.ToString() })
              .ToList();
        }
        public async Task<List<SelectListItem>> GetProductCategories()
        {
            return (await DatabaseContext.ProductCategories.ToListAsync())
                .Select(item => new SelectListItem { Text = item.Name, Value = item.Id.ToString() })
                .ToList();
        }
        public async Task<List<DeliveryType>> GetDeliveryTypes()
        {
            return await DatabaseContext.DeliveryTypes.ToListAsync();
        }
    }
}
