using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.UI.ViewModels.SharedViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class DeliveryViewModelMappings
    {
        public static List<DeliveryTypeViewModel> ConvertToDeliveryTypeViewModelList(this IEnumerable<DeliveryTypeResponse> items)
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
