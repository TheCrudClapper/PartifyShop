using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Mappings.ToDto;
using CSOS.Core.ServiceContracts;

namespace ComputerServiceOnlineShop.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(ICurrentUserService currentUserService, IAddressRepository addressRepository, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public async Task<Result> Edit(int id, AddressDto dto)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id);

            if (address == null)
                return Result.Failure(AddressErrors.AddressNotFound);

            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            address.CountryId = dto.CountryId;
            address.DateEdited = DateTime.UtcNow;
            address.Place = dto.Place;
            address.PostalCity = dto.PostalCity;
            address.PostalCode = dto.PostalCode;

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<EditAddressResponseDto>> GetAddressForEdit()
        {
            var userAndAddress = await GetCurrentUserAddress();

            if (userAndAddress == null)
                return Result.Failure<EditAddressResponseDto>(AddressErrors.AddressNotFound);

            return userAndAddress.ToEditAddresResponse();
        }

        public async Task<Result<UserAddresDetailsResponseDto>> GetUserAddresInfo()
        {
            var userAndAddress = await GetCurrentUserAddress();

            if (userAndAddress == null)
                return Result.Failure<UserAddresDetailsResponseDto>(AddressErrors.MissingAddressData);

            return userAndAddress.ToUserAddresDetailsResponse();
        }

        private async Task<ApplicationUser?> GetCurrentUserAddress()
        {
            var userId = _currentUserService.GetUserId();
            return await _accountRepository.GetUserWithAddressAsync(userId);
        }
    }
}
