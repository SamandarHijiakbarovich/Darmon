using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services
{
    public class PaymentService:IPaymentService
    {
        /*private readonly IPaymentRepository _paymentRepository;
        public async Task UpdateClickPaymentStatusAsync(int merchantTransId, bool isSuccess)
        {
            var payment = await _paymentRepository.GetByMerchantTransIdAsync(merchantTransId);
            if (payment == null)
                throw new NotFoundException($"Payment with MerchantTransId {merchantTransId} not found");

            payment.Status = isSuccess ? PaymentStatus.Completed : PaymentStatus.Failed;
            payment.UpdatedAt = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task<Payment> GetPaymentByMerchantTransIdAsync(int merchantTransId)
        {
            var payment = await _paymentRepository.GetByMerchantTransIdAsync(merchantTransId, true);
            if (payment == null)
                throw new NotFoundException($"Payment with MerchantTransId {merchantTransId} not found");

            return payment;
        }
        */
    }
}
