using ComputerServiceOnlineShop.Entities.Contexts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.Core.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Core.Services
{
    public class DeliveryTypeGetterService : IDeliveryTypeGetterService
    {
        public readonly DatabaseContext _databaseContext;
        public DeliveryTypeGetterService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<List<SelectListItemDto>> GetAllDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes.Where(item => item.IsActive)
                .Select(item => new SelectListItemDto
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),

                }).ToListAsync();
        }

        public async Task<List<SelectListItemDto>> GetOtherDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes
                .Where(item => item.IsActive)
                .Where(item => !item.Title.Contains("Locker"))
                .Select(item => new SelectListItemDto { Text = item.Title, Value = item.Id.ToString() })
                .ToListAsync();
        }

        public async Task<List<DeliveryTypeResponseDto>> GetParcelLockerDeliveryTypes()
        {
            return await _databaseContext.DeliveryTypes
                .Where(item => item.IsActive)
                .Where(item => item.Title.Contains("Locker"))
                .Select(item => new DeliveryTypeResponseDto()
                {
                    Id = item.Id,
                    Price = item.Price,
                    Title = item.Title,
                })
                .ToListAsync();
        }
    }
}
