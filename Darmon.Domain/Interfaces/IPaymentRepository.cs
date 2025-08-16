using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IPaymentRepository:IRepository<Payment>
{
    /// <summary>
    /// Get payment by ID with optional related entities
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <param name="includeRelated">Include transactions and order</param>
    /// <returns>Payment entity or null</returns>
    Task<Payment?> GetByIdAsync(Guid id, bool includeRelated = false);

    /// <summary>
    /// Get all payments with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="includeRelated">Include transactions and order</param>
    /// <returns>List of payments</returns>
    Task<IEnumerable<Payment>> GetAllAsync(int pageNumber = 1, int pageSize = 10, bool includeRelated = false);

    /// <summary>
    /// Add new payment
    /// </summary>
    /// <param name="payment">Payment to add</param>
    /// <returns>Added payment</returns>
    Task<Payment> AddAsync(Payment payment);

    /// <summary>
    /// Update existing payment
    /// </summary>
    /// <param name="payment">Payment to update</param>
    Task UpdateAsync(Payment payment);

    /// <summary>
    /// Delete payment by ID
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Get payments by order ID
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <returns>List of payments for the order</returns>
    Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId);


}
