using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.WithdrawHistoryDtos;

public class CreateWithdrawHistoryDto
{
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string BankAccount { get; set; }

    [Required]
    public int SellerWalletId { get; set; }
}
