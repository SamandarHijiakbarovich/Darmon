using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class FiscalSubmitRequestDto
    {
        public int ServiceId { get; set; }
        public long PaymentId { get; set; }
        public List<FiscalItemDto> Items { get; set; } = new();
        public int ReceivedEcash { get; set; }
        public int ReceivedCash { get; set; }
        public int ReceivedCard { get; set; }
    }
}
