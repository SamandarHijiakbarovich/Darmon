using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CourierDtos;

public class CreateCourierDto
{
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
