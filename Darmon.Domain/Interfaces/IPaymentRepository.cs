using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    /// <summary>
    /// Get payment by ID with optional related entities
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <param name="includeRelated">Include transactions and order</param>
    /// <returns>Payment entity or null</returns>
    Task<Payment?> GetByIdAsync(int id, bool includeRelated = false);

   

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
    Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);

    /// <summary>
    /// Get payment by Merchant Transaction ID (Click uchun)
    /// </summary>
    /// <param name="merchantTransId">Merchant Transaction ID</param>
    /// <returns>Payment entity or null</returns>
    Task<Payment?> GetByMerchantTransIdAsync(int merchantTransId);

    /// <summary>
    /// Get payment by Click Transaction ID
    /// </summary>
    /// <param name="clickTransId">Click Transaction ID</param>
    /// <returns>Payment entity or null</returns>
    Task<Payment?> GetByClickTransIdAsync(int clickTransId);

    /// <summary>
    /// Get payments by status
    /// </summary>
    /// <param name="status">Payment status</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of payments with specified status</returns>
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Get payments within date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="includeRelated">Include related entities</param>
    /// <returns>List of payments in date range</returns>
    Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, bool includeRelated = false);

    /// <summary>
    /// Check if payment exists by Merchant Transaction ID
    /// </summary>
    /// <param name="merchantTransId">Merchant Transaction ID</param>
    /// <returns>True if exists</returns>
    Task<bool> ExistsByMerchantTransIdAsync(int merchantTransId);

    /// <summary>
    /// Get total count of payments by status
    /// </summary>
    /// <param name="status">Payment status</param>
    /// <returns>Count of payments</returns>
    Task<int> GetCountByStatusAsync(PaymentStatus status);

    /// <summary>
    /// Get total revenue amount
    /// </summary>
    /// <param name="startDate">Start date (optional)</param>
    /// <param name="endDate">End date (optional)</param>
    /// <returns>Total revenue amount</returns>
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
}