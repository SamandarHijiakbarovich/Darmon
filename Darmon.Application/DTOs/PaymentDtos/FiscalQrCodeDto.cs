using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class FiscalQrCodeDto
    {
        public int ErrorCode { get; set; }
        public string ErrorNote { get; set; } = default!;
    }
}
