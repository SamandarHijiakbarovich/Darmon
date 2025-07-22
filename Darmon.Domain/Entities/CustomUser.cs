using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class CustomUser:BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; } = UserRole.User.ToString();

    public string PhoneNumber { get; set; }

    public string PasswordHash { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
