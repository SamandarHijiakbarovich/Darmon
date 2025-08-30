using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Branch: AuditableEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = default!;

    public int? AddressId { get; set; }
    public Address? Address { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public bool IsActive { get; set; } = true;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Relations
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
