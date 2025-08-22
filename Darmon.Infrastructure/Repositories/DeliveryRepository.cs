using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeliveryRepository> _logger;

    public DeliveryRepository(AppDbContext context, ILogger<DeliveryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Delivery> AddAsync(Delivery delivery)
    {
        try
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding delivery for order {OrderId}", delivery.OrderId);
            throw;
        }
    }

    public async Task<Delivery?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Courier)
                .Include(d => d.DeliveryAddress)
                .Include(d => d.StatusHistory)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving delivery with ID {DeliveryId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(Delivery delivery)
    {
        try
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating delivery with ID {DeliveryId}", delivery.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var delivery = await GetByIdAsync(id);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting delivery with ID {DeliveryId}", id);
            throw;
        }
    }

    public async Task<Delivery?> GetByOrderIdAsync(int orderId)
    {
        try
        {
            return await _context.Deliveries
                .Include(d => d.Courier)
                .Include(d => d.DeliveryAddress)
                .Include(d => d.StatusHistory)
                .FirstOrDefaultAsync(d => d.OrderId == orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving delivery for order {OrderId}", orderId);
            throw;
        }
    }

    public async Task<List<Delivery>> GetByCourierIdAsync(int courierId)
    {
        try
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryAddress)
                .Where(d => d.CourierId == courierId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deliveries for courier {CourierId}", courierId);
            throw;
        }
    }

    public async Task<List<Delivery>> GetByStatusAsync(DeliveryStatus status)
    {
        try
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Courier)
                .Where(d => d.Status == status)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deliveries with status {Status}", status);
            throw;
        }
    }

    public async Task<List<Delivery>> GetActiveDeliveriesAsync()
    {
        try
        {
            var activeStatuses = new[] { DeliveryStatus.Accepted, DeliveryStatus.Preparing, DeliveryStatus.OnTheWay, DeliveryStatus.Arrived };

            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Courier)
                .Include(d => d.DeliveryAddress)
                .Where(d => activeStatuses.Contains(d.Status))
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active deliveries");
            throw;
        }
    }

    public async Task AddStatusHistoryAsync(DeliveryStatusHistory statusHistory)
    {
        try
        {
            await _context.DeliveryStatusHistories.AddAsync(statusHistory);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding status history for delivery {DeliveryId}", statusHistory.DeliveryId);
            throw;
        }
    }

    public async Task<List<DeliveryStatusHistory>> GetStatusHistoryAsync(int deliveryId)
    {
        try
        {
            return await _context.DeliveryStatusHistories
                .Include(h => h.ChangedBy)
                .Where(h => h.DeliveryId == deliveryId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status history for delivery {DeliveryId}", deliveryId);
            throw;
        }
    }

    public async Task<int> GetCountByStatusAsync(DeliveryStatus status)
    {
        try
        {
            return await _context.Deliveries
                .CountAsync(d => d.Status == status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting delivery count for status {Status}", status);
            throw;
        }
    }

    public async Task<decimal> GetTotalDeliveryFeesAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return await _context.Deliveries
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate && d.Status == DeliveryStatus.Delivered)
                .SumAsync(d => d.DeliveryFee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total delivery fees from {StartDate} to {EndDate}", startDate, endDate);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            return await _context.Deliveries.AnyAsync(d => d.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of delivery {DeliveryId}", id);
            throw;
        }
    }

    public async Task<List<Delivery>> GetDeliveriesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Courier)
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deliveries from {StartDate} to {EndDate}", startDate, endDate);
            throw;
        }
    }
}