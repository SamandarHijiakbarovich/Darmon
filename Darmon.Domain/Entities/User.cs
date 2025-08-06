using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class User : AuditableEntity
{
    // Asosiy maydonlar
    [MaxLength(50)] public string FirstName { get; set; }
    [MaxLength(50)] public string LastName { get; set; }
    [MaxLength(20)] public string PhoneNumber { get; set; }
    [MaxLength(100)] public string Email { get; set; }
    public UserRole Role { get; set; }

    // Autentifikatsiya maydonlari
    public string PasswordHash { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }

    // Navigation properties
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<ProductReview> Reviews { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public SellerWallet? SellerWallet { get; set; } // CustomUserdagi xususiyat
}
