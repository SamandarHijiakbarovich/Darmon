using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryDtos;

public class DeliveryDto
{
    public int Id { get; set; }
    public DateTime EstimatedDeliveryTime { get; set; }
    public DateTime? ActualDeliveryTime { get; set; }
    public DeliveryStatus Status { get; set; }
    public string TrackingNumber { get; set; }

    public int AddressId { get; set; }
    public int? DeliveryPersonId { get; set; }
    public int OrderId { get; set; }
}
