using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Addresses;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Exceptions;
using CSOS.Core.ServiceContracts;

namespace ComputerServiceOnlineShop.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(ICurrentUserService currentUserService, IAddressRepository addressRepository, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Edit(int id, AddressDto dto)
        {
            var address = await _addressRepository.GetAddress(id);

            if(address == null)
                throw new EntityNotFoundException("Enitity not found");

            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            address.CountryId = dto.CountryId;
            address.DateEdited = DateTime.Now;
            address.Place = dto.Place;
            address.PostalCity = dto.PostalCity;
            address.PostalCode = dto.PostalCode;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Result<EditAddressResponseDto>> GetAddressForEdit()
        {
            var userId = _currentUserService.GetUserId();
            var userAndAddress = await _addressRepository.GetUserWithAddress(userId);

            if (userAndAddress == null)
                return Result.Failure<EditAddressResponseDto>(AddressErrors.AddressNotFound);

            return new EditAddressResponseDto()
            {
                Id = userAndAddress.AdressId,
                HouseNumber = userAndAddress.Address.HouseNumber,
                Place = userAndAddress.Address.Place,
                PostalCity = userAndAddress.Address.PostalCity,
                PostalCode = userAndAddress.Address.PostalCode,
                Street = userAndAddress.Address.Street,
                SelectedCountry = userAndAddress.Address.CountryId.ToString(),
            };
        }

        public async Task<UserAddresDetailsResponseDto> GetUserAddresInfo()
        {
            var userId = _currentUserService.GetUserId();
            var userAndAddress = await _addressRepository.GetUserWithAddress(userId);

            if (userAndAddress == null)
                throw new EntityNotFoundException("Enitity not found");

            return new UserAddresDetailsResponseDto()
            {
                CustomerName = userAndAddress.FirstName + " " + userAndAddress.Surname,
                PostalInfo = userAndAddress.Address.PostalCode + " " + userAndAddress.Address.PostalCity,
                AddressId = userAndAddress.AdressId,
                Address = userAndAddress.Address.Place + " " + userAndAddress.Address.HouseNumber,
                PhoneNumber = userAndAddress.PhoneNumber,
            };
        }
    }
}
