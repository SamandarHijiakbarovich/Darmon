using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class User : AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public UserRole Role { get; set; }
    
    // Navigation properties
   public int AddressId { get; set; }
    public Address Address { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<ProductReview> Reviews { get; set; }
     public ICollection<CartItem> CartItems { get; set; }
     //public ICollection<Notification> Notifications { get; set; }
}
