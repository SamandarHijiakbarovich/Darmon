using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Payment : AuditableEntity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "UZS"; // Default qiymat
    public PaymentStatus Status { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpirationDate { get; set; }

    // Relations
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    // Helper properties
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate < DateTime.UtcNow;
    public PaymentTransaction? LastTransaction => PaymentTransactions.OrderByDescending(t => t.CreatedAt).FirstOrDefault();
}

