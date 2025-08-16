using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryDtos;

public class CreateDeliveryDto
{
    public DateTime EstimatedDeliveryTime { get; set; }
    public DeliveryStatus Status { get; set; }
    public string TrackingNumber { get; set; }

    public int AddressId { get; set; }
    public int? DeliveryPersonId { get; set; }
    public int OrderId { get; set; }
}
