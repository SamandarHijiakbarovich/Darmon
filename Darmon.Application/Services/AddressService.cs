using AutoMapper;
using Darmon.Application.DTOs.AddressDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Darmon.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IAddressRepository addressRepository, IMapper mapper, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddressDto> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(id);
                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving address with ID {AddressId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId)
        {
            try
            {
                var addresses = await _addressRepository.GetByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<AddressDto>>(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving addresses for user {UserId}", userId);
                throw;
            }
        }

        public async Task<AddressDto> CreateAddressAsync(CreateAddressDto dto)
        {
            try
            {
                var address = _mapper.Map<Address>(dto);
                await _addressRepository.AddAsync(address);
                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating address for user {UserId}", dto.UserId);
                throw;
            }
        }

        public async Task<AddressDto> UpdateAddressAsync(int id, UpdateAddressDto dto)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(id);
                if (address == null)
                {
                    _logger.LogWarning("Address with ID {AddressId} not found for update", id);
                    return null;
                }

                _mapper.Map(dto, address);
                await _addressRepository.UpdateAsync(address);

                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address with ID {AddressId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(id);
                if (address == null)
                {
                    _logger.LogWarning("Address with ID {AddressId} not found for delete", id);
                    return false;
                }

                await _addressRepository.DeleteAsync(address);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address with ID {AddressId}", id);
                throw;
            }
        }

        public async Task<bool> SetDefaultAddressAsync(int addressId, int userId)
        {
            try
            {
                return await _addressRepository.SetDefaultAddressAsync(addressId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting default address {AddressId} for user {UserId}", addressId, userId);
                throw;
            }
        }

        public async Task<AddressDto> GetDefaultAddressAsync(int userId)
        {
            try
            {
                var address = await _addressRepository.GetDefaultAddressAsync(userId);
                return _mapper.Map<AddressDto>(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving default address for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<AddressDto>> SearchAddressesAsync(int userId, string searchTerm)
        {
            try
            {
                var addresses = await _addressRepository.SearchAddressesAsync(userId, searchTerm);
                return _mapper.Map<IEnumerable<AddressDto>>(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching addresses for user {UserId} with term {SearchTerm}", userId, searchTerm);
                throw;
            }
        }
    }
}
