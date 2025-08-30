using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces
{
    public interface IBranchRepository:IRepository<Branch>
    {
        // Pharmacy uchun maxsus metodlar
        Task<IEnumerable<Branch>> GetActivePharmaciesAsync();
        Task<IEnumerable<Branch>> GetNearbyPharmaciesAsync(double latitude, double longitude, double radiusKm);
        Task<IEnumerable<Branch>> SearchPharmaciesAsync(string searchTerm);
        Task<IEnumerable<Branch>> GetPharmaciesByWorkingHoursAsync(TimeSpan time);
        Task<Branch?> GetPharmacyWithProductsAsync(int pharmacyId);
        Task<bool> IsPharmacyOpenAsync(int pharmacyId, DateTime dateTime);

        // Product availability
        Task<bool> CheckProductAvailabilityAsync(int pharmacyId, int productId, int quantity);
        Task<Dictionary<int, int>> GetProductStockAsync(int pharmacyId); // productId -> stockQuantity
    }
}
