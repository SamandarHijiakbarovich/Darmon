using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Order : AuditableEntity
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }

    public int CourierId { get; set; }
    public Courier Courier { get; set; }


    // Relations
    public int UserId { get; set; }
    public User User { get; set; }

    public int? DeliveryId { get; set; }
    public Delivery? Delivery { get; set; }

    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
