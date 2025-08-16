using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.WithdrawHistoryDtos;

public class WithdrawHistoryDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public WithdrawStatus Status { get; set; }
    public string BankAccount { get; set; }
    public int SellerWalletId { get; set; }
    public DateTime CreatedAt { get; set; } // from AuditableEntity
}
