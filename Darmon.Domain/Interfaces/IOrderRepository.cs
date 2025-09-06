using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        // --- USER ---
        Task<Order> CreateOrderAsync(Order order);                // Yangi buyurtma yaratish
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);  // Mening buyurtmalarim
        Task CancelOrderAsync(int orderId, int userId);           // Buyurtmani bekor qilish

        // --- SELLER ---
        Task<IEnumerable<Order>> GetSellerOrdersAsync(int sellerId); // Sotuvchining buyurtmalari
        Task AcceptOrderAsync(int orderId, int sellerId);            // Buyurtmani qabul qilish
        Task RejectOrderAsync(int orderId, int sellerId);            // Buyurtmani rad etish
        Task MarkAsReadyAsync(int orderId, int sellerId);            // Buyurtma tayyor deb belgilash

        // --- COURIER ---
        Task<IEnumerable<Order>> GetCourierOrdersAsync(int courierId); // Kuryer buyurtmalari
        Task PickupOrderAsync(int orderId, int courierId);             // Buyurtmani olib ketish
        Task DeliverOrderAsync(int orderId, int courierId);            // Buyurtmani yetkazib berish
        
    }
}
