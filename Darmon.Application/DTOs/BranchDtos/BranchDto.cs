using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.BranchDtos;

public class BranchDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int? AddressId { get; set; }
    public string? AddressName { get; set; } // optional: Address.Name yoki Address.ToString()
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
}
