using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

public class Address:BaseEntity
{
    public string City { get; set; }           // Viloyat yoki shahar (masalan, Toshkent)
    public string District { get; set; }         // Tuman (masalan, Chilonzor)
    public string Street { get; set; }           // Ko‘cha nomi
    public string HouseNumber { get; set; }      // Uy raqami
    public string? Landmark { get; set; }        // Mo‘ljal (ixtiyoriy)

    public double? Latitude { get; set; }        // GPS koordinatalar (nullable)
    public double? Longitude { get; set; }

    public bool IsDefault { get; set; }          // Asosiy manzil flagi

    // Navigation properties
    public int UserId { get; set; }
    public User User { get; set; }

    public int? BranchId { get; set; }           // Nullable filial
    public Branch? Branch { get; set; }

}