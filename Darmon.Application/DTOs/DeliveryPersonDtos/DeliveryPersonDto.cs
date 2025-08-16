using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryPersonDtos;

public class DeliveryPersonDto
{
    public int Id { get; set; } // inherited from User
    public string FullName { get; set; } // optional, if User has it
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }
    public VehicleType VehicleType { get; set; }
}
