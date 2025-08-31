using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AddressDtos;

public class CreateAddressDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string District { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string HouseNumber { get; set; }

    public string? Landmark { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool IsDefault { get; set; }

    public int? BranchId { get; set; }
}
