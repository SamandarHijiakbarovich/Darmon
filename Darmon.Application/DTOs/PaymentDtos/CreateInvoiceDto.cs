using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos;

public class CreateInvoiceDto
{
    public int ServiceId { get; set; }
    public float Amount { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string MerchantTransId { get; set; } = default!;
}
