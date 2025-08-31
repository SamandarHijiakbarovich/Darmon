using Darmon.Application.DTOs.DeliveryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces
{
    public interface IDeliveryService
    {
        // ASOSIY METHODLAR
        Task<DeliveryDto> CreateDeliveryAsync(CreateDeliveryDto dto);
        Task<DeliveryDto> GetDeliveryByIdAsync(int id);
        Task<DeliveryDto> GetDeliveryByOrderIdAsync(int orderId);
        Task<DeliveryDto> UpdateDeliveryStatusAsync(int deliveryId, UpdateDeliveryDto dto);

        // KURIER METHODLARI
        Task<bool> AssignCourierAsync(int deliveryId, int courierId);
        Task<IEnumerable<DeliveryDto>> GetActiveDeliveriesAsync();

        // NARX HISOBLASH
        Task<decimal> CalculateDeliveryFeeAsync(int addressId, int branchId);
    }
}
