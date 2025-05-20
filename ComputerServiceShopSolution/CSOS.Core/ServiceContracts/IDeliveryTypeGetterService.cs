using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ServiceContracts
{
    public interface IDeliveryTypeGetterService
    {
        /// <summary>
        /// Gets an list of parcel locker delivery types 
        /// </summary>
        /// <returns>An list of DeliveryTypeViewModel</returns>
        Task<List<DeliveryTypeResponseDto>> GetParcelLockerDeliveryTypes();
        /// <summary>
        /// Gets delivery types, without parcel locker deliveries, as SelectListItem obj
        /// </summary>
        /// <returns>An List of SelctListItem type</returns>
        Task<List<SelectListItemDto>> GetOtherDeliveryTypes();
        /// <summary>
        /// Gets all active delivery types avaliable in database
        /// </summary>
        /// <returns>An List of SelectListItem</returns>
        Task<List<SelectListItemDto>> GetAllDeliveryTypes();
    }
}
