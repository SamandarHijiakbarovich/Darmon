using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ClickDtos.RequestResponseDto;

public class RequestResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class RequestResponseDto<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
}


