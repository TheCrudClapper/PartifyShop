using CSOS.Core.DTO;
using CSOS.Core.DTO.Requests;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.Core.ErrorHandling;

namespace CSOS.Core.ServiceContracts
{
    public interface IAddressService
    {
        /// <summary>
        /// Updates the user's address with the specified ID.
        /// </summary>
        /// <param name="id">ID of the address to edit.</param>
        /// <param name="dto">Updated address data.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> Edit(int id, AddressDto dto);

        /// <summary>
        /// Retrieves address details for editing by the currently logged-in user.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing <see cref="EditAddressResponseDto"/> if successful.
        /// </returns>
        Task<Result<EditAddressResponseDto>> GetAddressForEdit();

        /// <summary>
        /// Retrieves address information of the currently logged-in user.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing <see cref="UserAddresDetailsResponseDto"/> if successful.
        /// </returns>
        Task<Result<UserAddresDetailsResponseDto>> GetUserAddresInfo();
    }
}
