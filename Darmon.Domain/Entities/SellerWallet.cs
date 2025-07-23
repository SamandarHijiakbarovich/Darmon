using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class SellerWallet:BaseEntity
{
    public decimal Balance { get; set; }

        // Relations
    public string UserId { get; set; }
    public CustomUser User { get; set; }

    public ICollection<WithdrawHistory> WithdrawHistories { get; set; }
}
