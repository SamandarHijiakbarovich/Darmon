using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentDtos
{
    public class FiscalItemDto
    {
        public string Name { get; set; } = default!;         // Mahsulot nomi
        public int Quantity { get; set; }                    // Miqdor (dona)
        public int Price { get; set; }                       // Narx (so‘mda, 1 dona uchun)
        public int Sum { get; set; }                         // Umumiy summa = Quantity * Price
        public int Tax { get; set; }                         // Soliq (masalan, 0 yoki 12%)
        public CommissionInfoDto Commission { get; set; } = new(); // Komissiya ma’lumotlari
    }
}
