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

public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentTransactionRepository> _logger;

    public PaymentTransactionRepository(AppDbContext context, ILogger<PaymentTransactionRepository> logger) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaymentTransaction> AddAsync(PaymentTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        try
        {
            await _context.PaymentTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding payment transaction");
            throw new RepositoryException("Could not add payment transaction", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding payment transaction");
            throw;
        }
    }

    public async Task<PaymentTransaction?> GetByIdAsync(Guid id, bool includeGateway = true)
    {
        try
        {
            var query = _context.PaymentTransactions.AsQueryable();

            if (includeGateway)
            {
                query = query.Include(t => t.ClickTransactions);
            }

            return await query.FirstOrDefaultAsync(t => t.PaymentId == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment transaction with ID {TransactionId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(PaymentTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        try
        {
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating payment transaction with ID {TransactionId}", transaction.Id);
            throw new RepositoryException("Payment transaction update concurrency error", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating payment transaction with ID {TransactionId}", transaction.Id);
            throw new RepositoryException("Could not update payment transaction", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating payment transaction with ID {TransactionId}", transaction.Id);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentTransaction>> GetByPaymentIdAsync(Guid paymentId, bool includeGateway = true)
    {
        try
        {
            var query = _context.PaymentTransactions
                .Where(t => t.PaymentId == paymentId)
                .AsQueryable();

            if (includeGateway)
            {
                query = query.Include(t => t.ClickTransactions);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment transactions for payment ID {PaymentId}", paymentId);
            throw;
        }
    }

    public async Task<IEnumerable<PaymentTransaction>> GetByStatusAsync(TransactionStatus status, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            return await _context.PaymentTransactions
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment transactions with status {Status}", status);
            throw;
        }
    }

   

    public async Task<PaymentTransaction> GetByInternalTraceIdAsync(string internalTraceId)
    {
        try
        {
            return await _context.PaymentTransactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.InternalTraceId == internalTraceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"InternalTraceId: {internalTraceId} bo'yicha tranzaksiyani olishda xato");
            throw; // Yoki null qaytarishingiz mumkin
        }
    }

   
}