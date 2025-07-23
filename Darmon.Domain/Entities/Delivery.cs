using Darmon.Domain.Entities;

internal class Delivery:AuditableEntity
{
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public DeliveryStatus Status { get; set; }
        public string TrackingNumber { get; set; }

        // Relations
        public int AddressId { get; set; }
        public Address DeliveryAddress { get; set; }

        public int? DeliveryPersonId { get; set; }
        public DeliveryPerson? DeliveryPerson { get; set; }
}