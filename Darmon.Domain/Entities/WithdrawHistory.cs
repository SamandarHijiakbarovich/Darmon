using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class WithdrawHistory:BaseEntity
{
   
        [Key]
        public int Id { get; set; }

        public int WalletId { get; set; }
        public SellerWallet Wallet { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    
}
