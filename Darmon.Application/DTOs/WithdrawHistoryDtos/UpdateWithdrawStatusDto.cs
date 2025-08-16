using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.WithdrawHistoryDtos;

public class UpdateWithdrawStatusDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public WithdrawStatus Status { get; set; }
}
