using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class CardTokenVerifyDto
    {
        public int ServiceId { get; set; }
        public string CardToken { get; set; } = default!;
        public int SmsCode { get; set; }
    }
}
