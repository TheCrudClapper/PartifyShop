using CSOS.Core.Helpers;

namespace CSOS.Core.DTO.OfferDto
{
    public class OfferIndexResponse
    {
        public List<OfferResponse> Items = new List<OfferResponse>();
        public OfferFilter Filter { get; set; } = null!;

        public List<SelectListItemDto> DeliveryOptions = new List<SelectListItemDto>();

        public List<SelectListItemDto> SortingOptions = new List<SelectListItemDto>();
    }
}
