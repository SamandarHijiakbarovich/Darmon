using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IDeliveryRepository:IRepository<Delivery>
{
    Task<Delivery?> GetByOrderIdAsync(int orderId);
    Task<List<Delivery>> GetByCourierIdAsync(int courierId);
    Task<List<Delivery>> GetByStatusAsync(DeliveryStatus status);
    Task<List<Delivery>> GetActiveDeliveriesAsync();
    Task AddStatusHistoryAsync(DeliveryStatusHistory statusHistory);
    Task<List<DeliveryStatusHistory>> GetStatusHistoryAsync(int deliveryId);
    Task<int> GetCountByStatusAsync(DeliveryStatus status);
    Task<decimal> GetTotalDeliveryFeesAsync(DateTime startDate, DateTime endDate);
    Task<List<Delivery>> GetDeliveriesByDateRangeAsync(DateTime startDate, DateTime endDate);
}