using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // --- USER ---
        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Status = OrderStatus.Pending; // Yangi buyurtma avtomatik Pending
            await _context.Orders.AddAsync(order);
            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CancelOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not yours.");

            order.Status = OrderStatus.Cancelled;
            _context.Orders.Update(order);
        }

        // --- SELLER ---
        public async Task<IEnumerable<Order>> GetSellerOrdersAsync(int sellerId)
        {
            return await _context.Orders
                .Where(o => o.UserId == sellerId && o.User.Role == UserRole.PharmacyStaff)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AcceptOrderAsync(int orderId, int sellerId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == sellerId && o.User.Role == UserRole.PharmacyStaff);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not assigned to pharmacy staff.");

            order.Status = OrderStatus.Confirmed;
            _context.Orders.Update(order);
        }

        public async Task RejectOrderAsync(int orderId, int sellerId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == sellerId && o.User.Role == UserRole.PharmacyStaff);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not assigned to pharmacy staff.");

            order.Status = OrderStatus.Cancelled;
            _context.Orders.Update(order);
        }

        public async Task MarkAsReadyAsync(int orderId, int sellerId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == sellerId && o.User.Role == UserRole.PharmacyStaff);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not assigned to pharmacy staff.");

            order.Status = OrderStatus.Processing; // Ready statusni Processing bilan moslashtirish mumkin
            _context.Orders.Update(order);
        }

        // --- COURIER ---
        public async Task<IEnumerable<Order>> GetCourierOrdersAsync(int courierId)
        {
            return await _context.Orders
                .Where(o => o.CourierId == courierId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task PickupOrderAsync(int orderId, int courierId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CourierId == courierId);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not assigned to courier.");

            order.Status = OrderStatus.Processing; // Pickup = Processing
            _context.Orders.Update(order);
        }

        public async Task DeliverOrderAsync(int orderId, int courierId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CourierId == courierId);

            if (order == null)
                throw new KeyNotFoundException("Order not found or not assigned to courier.");

            order.Status = OrderStatus.Delivered;
            _context.Orders.Update(order);
        }

       
    }
}
