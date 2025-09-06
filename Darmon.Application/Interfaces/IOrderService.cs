using Darmon.Application.DTOs.OrderDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces
{
    public interface IOrderService
    {
        // --------------------
        // Asosiy CRUD
        // --------------------

        // Buyurtma yaratish (Customer tomonidan)
        Task<OrderDto> CreateAsync(CreateOrderDto dto);

        // Barcha buyurtmalarni olish (Admin yoki PharmacyStaff uchun)
        Task<IEnumerable<OrderDto>> GetAllAsync();

        // Faqat bitta buyurtmani olish
        Task<OrderDto> GetByIdAsync(int id);

        // Buyurtmani yangilash (status yoki boshqa maydonlar uchun)
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto dto);

        // Buyurtmani o‘chirish
        Task<bool> DeleteAsync(int id);

        // --------------------
        // Userga oid buyurtmalar
        // --------------------

        // Foydalanuvchining o‘z buyurtmalarini olish (Customer)
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);

        // Courier uchun faqat unga biriktirilgan buyurtmalarni olish
        Task<IEnumerable<OrderDto>> GetCourierOrdersAsync(int courierId);

        // --------------------
        // Order lifecycle (faqat tegishli rollar uchun)
        // --------------------

        // Buyurtmani qabul qilish (PharmacyStaff)
        Task<bool> AcceptOrderAsync(int orderId, int userId);

        // Buyurtmani rad etish (PharmacyStaff)
        Task<bool> RejectOrderAsync(int orderId, int userId);

        // Buyurtmani tayyor deb belgilash (PharmacyStaff)
        Task<bool> MarkAsReadyAsync(int orderId, int userId);

        // Buyurtmani Courierga biriktirish (Admin yoki PharmacyStaff)
        Task<bool> AssignCourierAsync(int orderId, int courierId);

        // Courier buyurtmani yetkazib berganini belgilaydi
        Task<bool> MarkAsDeliveredAsync(int orderId, int courierId);

        // --------------------
        // Qo‘shimcha querylar
        // --------------------

        // Status bo‘yicha buyurtmalarni olish (Admin uchun)
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status);

        // Sana bo‘yicha buyurtmalarni olish (masalan, so‘nggi 7 kunlik)
        Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(System.DateTime start, System.DateTime end);

        // Statistikalar (Admin uchun)
        Task<int> GetTotalOrdersCountAsync();
        Task<int> GetOrdersCountByStatusAsync(string status);
        Task<decimal> GetTotalRevenueAsync(); // jami tushum hisoblash
    }
}
