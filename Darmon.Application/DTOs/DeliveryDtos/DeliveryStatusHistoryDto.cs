using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryDtos
{
    public class DeliveryStatusHistoryDto
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Notes { get; set; }
        public int ChangedById { get; set; }
    }
}
