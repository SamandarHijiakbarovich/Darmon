using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryPersonDtos;

public class UpdateDeliveryPersonDto
{
    public int Id { get; set; }
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }
    public VehicleType VehicleType { get; set; }
}
