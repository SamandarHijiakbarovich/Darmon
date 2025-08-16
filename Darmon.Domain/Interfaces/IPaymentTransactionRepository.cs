using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IPaymentTransactionRepository:IRepository<PaymentTransaction>
{
    Task<PaymentTransaction> AddAsync(PaymentTransaction transaction);
    Task<PaymentTransaction?> GetByIdAsync(Guid id, bool includeGateway = true);
    Task UpdateAsync(PaymentTransaction transaction);
    Task<IEnumerable<PaymentTransaction>> GetByPaymentIdAsync(Guid paymentId, bool includeGateway = true);
    Task<IEnumerable<PaymentTransaction>> GetByStatusAsync(TransactionStatus status, int pageNumber = 1, int pageSize = 10);
    Task<PaymentTransaction> GetByInternalTraceIdAsync(string internalTraceId);
}
