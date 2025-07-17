using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.UI.ViewModels.SharedViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class DeliveryViewModelMappings
    {
        public static List<DeliveryTypeViewModel> ConvertToDeliveryTypeViewModelList(this IEnumerable<DeliveryTypeResponseDto> items)
        {
            return items.Select(item => new DeliveryTypeViewModel
            {
                Id = item.Id,
                Price = item.Price,
                Title = item.Title,
            })
             .ToList();
        }
    }
}
