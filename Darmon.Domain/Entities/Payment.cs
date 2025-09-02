using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Payment : AuditableEntity
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "UZS";
    public PaymentStatus Status { get; set; }
    public PaymentProvider Provider { get; set; } // Click, Payme, etc.
    public string? Description { get; set; }
    public DateTime? ExpirationDate { get; set; }

    // Click specific fields
    public int? MerchantTransId { get; set; } // Click uchun
    public int? ClickTransId { get; set; } // Click uchun
    public int? MerchantPrepareId { get; set; } // Click uchun

    // Payme specific fields
    public string? PaymeTransactionId { get; set; } // Payme uchun
    public long? PaymeTime { get; set; } // Payme uchun
    public int? PaymeState { get; set; } // Payme uchun

    // Common gateway fields
    public string? GatewayTransactionId { get; set; }
    public DateTime? GatewayTime { get; set; }
    public string? GatewayState { get; set; }
    public string? ErrorMessage { get; set; }
    public int? ErrorCode { get; set; }

    // Relations
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    // Helper properties
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate < DateTime.UtcNow;
    public bool IsPending => Status == PaymentStatus.Pending;
    public bool IsCompleted => Status == PaymentStatus.Completed;
    public bool IsFailed => Status == PaymentStatus.Failed;
    public bool IsCancelled => Status == PaymentStatus.Cancelled;

    public PaymentTransaction? LastTransaction =>
        PaymentTransactions.OrderByDescending(t => t.CreatedAt).FirstOrDefault();

}

