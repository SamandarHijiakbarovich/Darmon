using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AuthResponse;

public class UserRegisterDto
{
    // UserCreateDto.cs ichida
    [Required(ErrorMessage = "Ism kiritilishi shart")]
    [StringLength(50, ErrorMessage = "Ism 50 ta belgidan oshmasin")]
    public string FirstName { get; set; }

    // UserCreateDto.cs ichida
    [Required(ErrorMessage = "Familiya kiritilishi shart")]
    [StringLength(50, ErrorMessage = "Ism 50 ta belgidan oshmasin")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email kiriting")]
    public string Email { get; set; }

    public string Password { get; set; }
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}
