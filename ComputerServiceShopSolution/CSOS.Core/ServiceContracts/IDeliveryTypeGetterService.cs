using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;

namespace CSOS.Core.ServiceContracts
{
    public interface IDeliveryTypeGetterService
    {
        /// <summary>
        /// Retrieves a list of delivery types that are parcel locker options.
        /// </summary>
        /// <returns>A list of <see cref="DeliveryTypeResponseDto"/> representing parcel locker delivery types.</returns>
        Task<List<DeliveryTypeResponseDto>> GetParcelLockerDeliveryTypes();

        /// <summary>
        /// Retrieves delivery types that are not parcel locker options, returned as select list items.
        /// </summary>
        /// <returns>A list of <see cref="SelectListItemDto"/> representing other delivery types.</returns>
        Task<List<SelectListItemDto>> GetOtherDeliveryTypes();

        /// <summary>
        /// Retrieves all active delivery types available in the database.
        /// </summary>
        /// <returns>A list of <see cref="SelectListItemDto"/> representing all available delivery types.</returns>
        Task<List<SelectListItemDto>> GetAllDeliveryTypesAsSelectionList();
    }
}
