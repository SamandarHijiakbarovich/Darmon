using AutoMapper;
using Darmon.Application.DTOs.OrderDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // --------------------
        // CRUD
        // --------------------
        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var entity = _mapper.Map<Order>(dto);
            entity.Status = OrderStatus.Pending;
            entity.CreatedAt = DateTime.UtcNow;

            await _orderRepository.AddAsync(entity);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} created by User {UserId}", entity.Id, entity.UserId);

            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var list = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(list);
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateAsync(int id, UpdateOrderDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new KeyNotFoundException("Order not found");

            _mapper.Map(dto, order);
            _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} updated", id);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order with id {OrderId} not found", id);
                return false;
            }

            await _orderRepository.DeleteAsync(order);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} deleted", id);

            return true;
        }

        // --------------------
        // User Orders
        // --------------------
        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var list = await _orderRepository.GetUserOrdersAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(list);
        }

        public async Task<IEnumerable<OrderDto>> GetCourierOrdersAsync(int courierId)
        {
            var list = await _orderRepository.GetCourierOrdersAsync(courierId);
            return _mapper.Map<IEnumerable<OrderDto>>(list);
        }

        // --------------------
        // Lifecycle
        // --------------------
        public async Task<bool> AcceptOrderAsync(int orderId, int userId)
        {
            await _orderRepository.AcceptOrderAsync(orderId, userId);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} accepted by Staff {UserId}", orderId, userId);
            return true;
        }

        public async Task<bool> RejectOrderAsync(int orderId, int userId)
        {
            await _orderRepository.RejectOrderAsync(orderId, userId);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} rejected by Staff {UserId}", orderId, userId);
            return true;
        }

        public async Task<bool> MarkAsReadyAsync(int orderId, int userId)
        {
            await _orderRepository.MarkAsReadyAsync(orderId, userId);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} marked as Ready by Staff {UserId}", orderId, userId);
            return true;
        }

        public async Task<bool> AssignCourierAsync(int orderId, int courierId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new KeyNotFoundException("Order not found");

            order.CourierId = courierId;
            order.Status = OrderStatus.Processing;

            _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} assigned to Courier {CourierId}", orderId, courierId);
            return true;
        }

        public async Task<bool> MarkAsDeliveredAsync(int orderId, int courierId)
        {
            await _orderRepository.DeliverOrderAsync(orderId, courierId);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} delivered by Courier {CourierId}", orderId, courierId);
            return true;
        }

        // --------------------
        // Queries
        // --------------------
        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            var list = await _orderRepository.GetAllAsync();
            var filtered = list.Where(o => o.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
            return _mapper.Map<IEnumerable<OrderDto>>(filtered);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            var list = await _orderRepository.GetAllAsync();
            var filtered = list.Where(o => o.CreatedAt >= start && o.CreatedAt <= end);
            return _mapper.Map<IEnumerable<OrderDto>>(filtered);
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            var list = await _orderRepository.GetAllAsync();
            return list.Count();
        }

        public async Task<int> GetOrdersCountByStatusAsync(string status)
        {
            var list = await _orderRepository.GetAllAsync();
            return list.Count(o => o.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            var list = await _orderRepository.GetAllAsync();
            return list.Sum(o => o.TotalAmount);
        }
    }
}
