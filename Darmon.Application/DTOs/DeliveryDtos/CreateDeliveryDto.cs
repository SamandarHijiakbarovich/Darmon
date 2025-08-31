using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryDtos;

public class CreateDeliveryDto
{
    public int OrderId { get; set; }
    public int DeliveryAddressId { get; set; }
    public decimal DeliveryFee { get; set; }
    public string? Notes { get; set; }
}
