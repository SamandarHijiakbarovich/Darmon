using Darmon.Domain.Entities;
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

    public async Task<Payment?> GetByIdAsync(Guid id, bool includeRelated = false)
    {
        try
        {
            var query = _context.Payments.AsQueryable();

            if (includeRelated)
            {
                query = query.Include(p => p.Transactions)
                             .ThenInclude(t => t.GatewayTransactions)
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
                query = query.Include(p => p.Transactions)
                           .Include(p => p.Order);
            }

            return await query
                .OrderBy(p => p.CreatedAt)
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
            _context.Entry(payment).State = EntityState.Modified;
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

    public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
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

   
}