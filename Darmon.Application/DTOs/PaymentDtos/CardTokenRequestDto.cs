using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class CardTokenRequestDto
    {
        public int ServiceId { get; set; }
        public string CardNumber { get; set; } = default!;
        public string ExpireDate { get; set; } = default!; // Format: MMYY
        public bool Temporary { get; set; } // true = bir martalik token
    }
}
