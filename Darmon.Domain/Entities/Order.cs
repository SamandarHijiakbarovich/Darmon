using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class Order:BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid CourierId { get; set; }
    public Courier Courier { get; set; } = default!;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Payment Payment { get; set; } = default!;
}
