using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Darmon.Domain.Exceptions;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(AppDbContext context, ILogger<PaymentRepository> logger) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Payment?> GetByIdAsync(int id, bool includeRelated = false)
    {
        try
        {
            var query = _context.Payments.AsQueryable();

            if (includeRelated)
            {
                query = query.Include(p => p.PaymentTransactions)
                             .Include(p => p.Order);
            }

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment with ID {PaymentId}", id);
            throw;
        }
    }

   

    public async Task<IEnumerable<Payment>> GetAllAsync(int pageNumber = 1, int pageSize = 10, bool includeRelated = false)
    {
        try
        {
            var query = _context.Payments.AsQueryable();

            if (includeRelated)
            {
                query = query.Include(p => p.PaymentTransactions)
                             .Include(p => p.Order);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments");
            throw;
        }
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        try
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding payment");
            throw new RepositoryException("Could not add payment", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding payment");
            throw;
        }
    }

    public async Task UpdateAsync(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        try
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating payment with ID {PaymentId}", payment.Id);
            throw new RepositoryException("Payment update concurrency error", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating payment with ID {PaymentId}", payment.Id);
            throw new RepositoryException("Could not update payment", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating payment with ID {PaymentId}", payment.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error deleting payment with ID {PaymentId}", id);
            throw new RepositoryException("Could not delete payment", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting payment with ID {PaymentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for order {OrderId}", orderId);
            throw;
        }
    }

    public async Task<Payment?> GetByMerchantTransIdAsync(int merchantTransId)
    {
        try
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.MerchantTransId == merchantTransId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment by MerchantTransId {MerchantTransId}", merchantTransId);
            throw;
        }
    }

    public async Task<Payment?> GetByClickTransIdAsync(int clickTransId)
    {
        try
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.ClickTransId == clickTransId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment by ClickTransId {ClickTransId}", clickTransId);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments with status {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, bool includeRelated = false)
    {
        try
        {
            var query = _context.Payments
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);

            if (includeRelated)
            {
                query = query.Include(p => p.PaymentTransactions)
                             .Include(p => p.Order);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments from {StartDate} to {EndDate}", startDate, endDate);
            throw;
        }
    }

    public async Task<bool> ExistsByMerchantTransIdAsync(int merchantTransId)
    {
        try
        {
            return await _context.Payments
                .AnyAsync(p => p.MerchantTransId == merchantTransId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence by MerchantTransId {MerchantTransId}", merchantTransId);
            throw;
        }
    }

    public async Task<int> GetCountByStatusAsync(PaymentStatus status)
    {
        try
        {
            return await _context.Payments
                .CountAsync(p => p.Status == status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting count by status {Status}", status);
            throw;
        }
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed);

            if (startDate.HasValue)
                query = query.Where(p => p.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.CreatedAt <= endDate.Value);

            return await query.SumAsync(p => p.Amount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total revenue");
            throw;
        }
    }

    // Qo'shimcha metodlar - Payme va boshqa providerlar uchun
    public async Task<Payment?> GetByPaymeTransactionIdAsync(string paymeTransactionId)
    {
        try
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymeTransactionId == paymeTransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment by PaymeTransactionId {PaymeTransactionId}", paymeTransactionId);
            throw;
        }
    }

    public async Task<Payment?> GetByGatewayTransactionIdAsync(string gatewayTransactionId)
    {
        try
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.GatewayTransactionId == gatewayTransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment by GatewayTransactionId {GatewayTransactionId}", gatewayTransactionId);
            throw;
        }
    }

    public async Task<bool> ExistsByPaymeTransactionIdAsync(string paymeTransactionId)
    {
        try
        {
            return await _context.Payments
                .AnyAsync(p => p.PaymeTransactionId == paymeTransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence by PaymeTransactionId {PaymeTransactionId}", paymeTransactionId);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByProviderAsync(PaymentProvider provider, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.Provider == provider)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for provider {Provider}", provider);
            throw;
        }
    }

    public async Task<IEnumerable<Payment>> GetByStatusAndProviderAsync(PaymentStatus status, PaymentProvider provider, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Payments
                .Where(p => p.Status == status && p.Provider == provider)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payments for status {Status} and provider {Provider}", status, provider);
            throw;
        }
    }
}