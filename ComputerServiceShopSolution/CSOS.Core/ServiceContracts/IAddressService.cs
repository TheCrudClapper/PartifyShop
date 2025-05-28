using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface IAddressService
    {
        Task Edit(int id, AddressDto dto);
        Task<EditAddressResponseDto> GetAddressForEdit();

        Task<UserAddresDetailsResponseDto> GetUserAddresInfo();
    }
}
