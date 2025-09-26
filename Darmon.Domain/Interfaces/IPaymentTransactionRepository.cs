using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IPaymentTransactionRepository:IRepository<PaymentTransaction>
{


        Task<string> GenerateTransactionIdAsync();
        Task SaveChangesAsync(PaymentTransaction transaction);
        Task UpdateChangesAsync(PaymentTransaction transaction);
        Task<PaymentTransaction> GetTransactionByTransactionIdAsync(string transactionId);
 
}
