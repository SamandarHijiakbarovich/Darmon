using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities
{
    public class DeliveryStatusHistory
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }
        public Delivery Delivery { get; set; }   // Navigation property

        public DeliveryStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Notes { get; set; }

        public int ChangedById { get; set; }     // Kim o‘zgartirgan
        public User ChangedBy { get; set; }      // Navigation property
    }
}
