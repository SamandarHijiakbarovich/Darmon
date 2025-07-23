using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class WithdrawHistory : AuditableEntity
{
        public decimal Amount { get; set; }
        public WithdrawStatus Status { get; set; }
        public string BankAccount { get; set; }

        // Relations
        public int SellerWalletId { get; set; }
        public SellerWallet SellerWallet { get; set; }    
    
}
