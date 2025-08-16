using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.NotificationDtos;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public bool IsRead { get; set; }
}
