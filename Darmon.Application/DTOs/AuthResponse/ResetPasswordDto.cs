using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AuthResponse;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Token majburiy")]
    public string Token { get; set; }


    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
ErrorMessage = "Parol kamida 1 ta katta harf, 1 ta kichik harf, 1 ta raqam va 1 ta maxsus belgidan iborat bo'lishi kerak")]
    [Required(ErrorMessage = "Yangi parol majburiy")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parol kamida 8 ta belgidan iborat bo'lishi kerak")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Parolni tasdiqlash majburiy")]
    [Compare("NewPassword", ErrorMessage = "Parollar mos kelmayapti")]
    public string ConfirmPassword { get; set; }
}
