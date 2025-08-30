using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos;

public class InvoiceStatusResponseDto
{
    public int ErrorCode { get; set; }
    public string ErrorNote { get; set; } = default!;
    public long InvoiceId { get; set; }
    public int InvoiceStatus { get; set; }
    public string InvoiceStatusNote { get; set; } = default!;
}
