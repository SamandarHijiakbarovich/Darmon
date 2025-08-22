using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IDeliveryRepository:IRepository<Delivery>
{
    // Asosiy CRUD operatsiyalari
    Task<Delivery> AddAsync(Delivery delivery);
    Task<Delivery?> GetByIdAsync(int id);
    Task UpdateAsync(Delivery delivery);
    Task DeleteAsync(int id);

    // Query operatsiyalari
    Task<Delivery?> GetByOrderIdAsync(int orderId);
    Task<List<Delivery>> GetByCourierIdAsync(int courierId);
    Task<List<Delivery>> GetByStatusAsync(DeliveryStatus status);
    Task<List<Delivery>> GetActiveDeliveriesAsync();
    Task<List<Delivery>> GetDeliveriesByDateRangeAsync(DateTime startDate, DateTime endDate);

    // Status history operatsiyalari
 
    //Task AddStatusHistoryAsync(DeliveryStatusHistory statusHistory);
   
   
    //Task<List<DeliveryStatusHistory>> GetStatusHistoryAsync(int deliveryId);

    // Statistikalar
    Task<int> GetCountByStatusAsync(DeliveryStatus status);
    Task<decimal> GetTotalDeliveryFeesAsync(DateTime startDate, DateTime endDate);

    // Mavjudlik tekshirish
    Task<bool> ExistsAsync(int id);
}