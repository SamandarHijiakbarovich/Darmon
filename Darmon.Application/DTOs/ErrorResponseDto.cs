using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs;

public class ErrorResponseDto
{
    
        public string Message { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public ErrorResponseDto(string message)
        {
            Message = message;
        }
    
}
