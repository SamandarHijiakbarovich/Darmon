using Darmon.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CourierDtos;

public class CourierDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public List<OrderDto> Orders { get; set; }= new List<OrderDto>();
}
