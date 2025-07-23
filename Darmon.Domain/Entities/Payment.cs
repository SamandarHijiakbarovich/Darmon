using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class Payment : AuditableEntity
{
     public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public string TransactionId { get; set; }
    
    // Navigation properties
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public PaymentTransaction PaymentTransaction { get; set; }
}
