using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface IAddressService
    {
        Task Edit(int id, AddressDto dto);
        Task<EditAddressViewModel> GetAddressForEdit();

        Task<UserAddressDetailsViewModel> GetUserAddresInfo();
    }
}
