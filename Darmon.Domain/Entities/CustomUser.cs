using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class CustomUser : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

        // Relations
    public ICollection<Address> Addresses { get; set; }
    public ICollection<Order> Orders { get; set; }
    public SellerWallet? SellerWallet { get; set; }
}
