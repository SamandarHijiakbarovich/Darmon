using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class SellerWallet:BaseEntity
{
   
    public decimal Balance { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property
    public virtual User User { get; set; }

    public virtual ICollection<WithdrawHistory> WithdrawHistories { get; set; }
}
