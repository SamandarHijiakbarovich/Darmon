using Darmon.Domain.Entities;

internal class DeliveryPerson : User
{
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }

        // Relations
    public string UserId { get; set; }
    public CustomUser User { get; set; }

    public ICollection<Delivery> Deliveries { get; set; }
    public VehicleType VehicleType { get; set; } // enum { Car, Motorcycle, Bicycle, OnFoot }
}