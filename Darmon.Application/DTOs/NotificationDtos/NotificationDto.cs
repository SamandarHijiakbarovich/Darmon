using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.NotificationDtos;

public class NotificationDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }

    public int UserId { get; set; }
}
