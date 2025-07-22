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
    [Key]
    public int Id { get; set; }

    public Guid SellerId { get; set; }
    public CustomUser Seller { get; set; }

    public decimal Balance { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<WithdrawHistory> Withdraws { get; set; }
}
