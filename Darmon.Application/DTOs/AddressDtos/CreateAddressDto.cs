using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AddressDtos;

public class CreateAddressDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Landmark { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public int UserId { get; set; }
    public int? BranchId { get; set; }
}
