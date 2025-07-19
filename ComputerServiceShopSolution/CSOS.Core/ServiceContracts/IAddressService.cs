using CSOS.Core.DTO.AddressDto;
using CSOS.Core.ErrorHandling;

namespace CSOS.Core.ServiceContracts
{
    public interface IAddressService
    {
        /// <summary>
        /// Updates the user's address with the specified ID.
        /// </summary>
        /// <param name="request">Updated address data.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> EditUserAddress(AddressUpdateRequest? request);

        /// <summary>
        /// Retrieves address details for editing by the currently logged-in user.
        /// </summary>
        /// <returns>
        /// </returns>
        Task<Result<AddressResponse>> GetUserAddressForEdit();

        /// <summary>
        /// Retrieves address information of the currently logged-in user.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing <see cref="UserAddressDetailsResponseDto"/> if successful.
        /// </returns>
        Task<Result<UserAddressDetailsResponseDto>> GetUserAddressDetails();
    }
}
