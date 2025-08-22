using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Courier: User
{
    public string FullName { get; set; }
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }
    public VehicleType VehicleType { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Delivery> Deliveries { get; set; }
}
