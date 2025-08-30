using Darmon.Application.DTOs.GatewayTransactionDtos;
using Darmon.Application.DTOs.PaymentTransactionDtos;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IPaymentTransactionService
{
    Task<PaymentTransaction> CreateTransactionAsync(PaymentTransactionDto dto);
    Task<PaymentTransactionDto> GetTransactionByIdAsync(Guid id);
    Task<PaymentTransactionDto> UpdateTransactionAsync(PaymentTransactionDto dto);
    Task<IEnumerable<PaymentTransactionDto>> GetTransactionsByPaymentIdAsync(Guid paymentId);
    Task<IEnumerable<PaymentTransactionDto>> GetTransactionsByStatusAsync(TransactionStatus status, int pageNumber = 1, int pageSize = 10);
    Task<PaymentTransactionDto> GetByInternalTraceIdAsync(string internalTraceId);
    Task<bool> DeleteTransactionAsync(Guid id);
    Task GetAllAsync();
}
