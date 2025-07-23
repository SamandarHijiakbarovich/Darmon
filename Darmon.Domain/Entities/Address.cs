using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

internal class Address:BaseEntity
{
     public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Landmark { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Navigation properties
    public int UserId { get; set; }
    public User User { get; set; }
}