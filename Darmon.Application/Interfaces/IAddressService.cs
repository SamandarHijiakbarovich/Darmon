using Darmon.Application.DTOs.AddressDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IAddressService
{
    Task<AddressDto> GetAddressByIdAsync(int id);
    Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId);
    Task<AddressDto> CreateAddressAsync(CreateAddressDto dto);
    Task<AddressDto> UpdateAddressAsync(int id, UpdateAddressDto dto);
    Task<bool> DeleteAddressAsync(int id);
    Task<bool> SetDefaultAddressAsync(int addressId, int userId);
    Task<AddressDto> GetDefaultAddressAsync(int userId);
    Task<IEnumerable<AddressDto>> SearchAddressesAsync(int userId, string searchTerm);
}
