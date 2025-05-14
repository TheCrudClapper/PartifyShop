using CSOS.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.DTO.Responses.Offers
{
    public class OfferBrowserResponseDto
    {
        public List<OfferBrowserItemResponseDto> Items = new List<OfferBrowserItemResponseDto>();
        public OfferFilter Filter { get; set; }

        public List<SelectListItemDto> DeliveryOptions = new List<SelectListItemDto>();

        public List<SelectListItemDto> SortingOptions = new List<SelectListItemDto>();
    }
}
