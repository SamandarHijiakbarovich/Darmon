using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryPersonDtos;

public class CreateDeliveryPersonDto
{
    public string FullName { get; set; } // optional
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }
    public VehicleType VehicleType { get; set; }
}
