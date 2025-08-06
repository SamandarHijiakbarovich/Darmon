using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AuthResponse;

public class ForgotPasswordReques
{
    [Required(ErrorMessage = "Email kiritilishi shart")]
    [EmailAddress(ErrorMessage = "Noto'g'ri email formati")]
    public string Email { get; set; }
}
