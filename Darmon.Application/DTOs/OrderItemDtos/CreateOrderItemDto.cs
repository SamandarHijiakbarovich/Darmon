using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.OrderItemDtos;

public class CreateOrderItemDto
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int ProductId { get; set; }
}
