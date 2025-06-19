using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.Core.ErrorHandling;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface IAddressService
    {
        Task Edit(int id, AddressDto dto);
        Task<Result<EditAddressResponseDto>> GetAddressForEdit();

        Task<UserAddresDetailsResponseDto> GetUserAddresInfo();
    }
}
