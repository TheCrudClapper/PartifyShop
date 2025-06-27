using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;

namespace CSOS.Core.Services
{
    public class DeliveryTypeGetterService : IDeliveryTypeGetterService
    {
        private readonly IDeliveryTypeRepository _deliveryTypeRepository;

        public DeliveryTypeGetterService(IDeliveryTypeRepository deliveryTypeRepository)
        {
            _deliveryTypeRepository = deliveryTypeRepository;
        }

        public async Task<List<SelectListItemDto>> GetAllDeliveryTypes()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();

            return types.Select(item => new SelectListItemDto
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }).ToList();
        }

        public async Task<List<SelectListItemDto>> GetOtherDeliveryTypes()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();

            return types
                .Where(item => !item.Title.Contains("Locker"))
                .Select(item => new SelectListItemDto
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                })
                .ToList();
        }

        public async Task<List<DeliveryTypeResponseDto>> GetParcelLockerDeliveryTypes()
        {
            var types = await _deliveryTypeRepository.GetAllDeliveryTypesAsync();

            return types
                .Where(item => item.Title.Contains("Locker"))
                .Select(item => new DeliveryTypeResponseDto
                {
                    Id = item.Id,
                    Price = item.Price,
                    Title = item.Title
                })
                .ToList();
        }
    }
}