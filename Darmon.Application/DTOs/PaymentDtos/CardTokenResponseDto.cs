using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class CardTokenResponseDto
    {
        public int ErrorCode { get; set; }
        public string ErrorNote { get; set; } = default!;
        public string CardToken { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool Temporary { get; set; }
    }
}
