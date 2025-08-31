using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.DeliveryDtos
{
    public class DeliveryStatsDto
    {
        public int TotalDeliveries { get; set; }
        public int PendingDeliveries { get; set; }
        public int ActiveDeliveries { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
