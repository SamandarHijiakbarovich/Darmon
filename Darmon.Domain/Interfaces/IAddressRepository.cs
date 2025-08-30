using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces
{
    public interface IAddressRepository:IRepository<Address>
    {
        // Address uchun maxsus metodlar
        Task<IEnumerable<Address>> GetByUserIdAsync(int userId);
        Task<Address?> GetDefaultAddressAsync(int userId);
        Task<bool> SetDefaultAddressAsync(int addressId, int userId);
        Task<IEnumerable<Address>> SearchAddressesAsync(int userId, string searchTerm);
    }
}
