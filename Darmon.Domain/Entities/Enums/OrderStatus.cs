using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities.Enums;

internal enum OrderStatus
{
    Pending,
    Accepted,
    InDelivery,
    Delivered,
    Cancelled
}
