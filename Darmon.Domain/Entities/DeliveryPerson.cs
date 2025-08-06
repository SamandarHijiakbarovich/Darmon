using Darmon.Domain.Entities;

public class DeliveryPerson : User
{
    public string VehicleNumber { get; set; }
    public bool IsAvailable { get; set; }
    public VehicleType VehicleType { get; set; }

    // Navigation property
    public ICollection<Delivery> Deliveries { get; set; }
}