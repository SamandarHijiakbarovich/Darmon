using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities.Enums
{
    public enum TransactionStatus
    {
        Created,
        Pending,
        Processing,
        Completed,
        Failed,
        Retry,       // <-- mana shu qiymat
        Cancelled
    }
}
