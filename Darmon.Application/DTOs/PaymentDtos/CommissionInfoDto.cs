using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos;

public class CommissionInfoDto
{
    public float Percent { get; set; } = 2.0f;           // Komissiya foizi (2%)
    public int Amount { get; set; }                      // Komissiya summasi (so‘mda)
}
