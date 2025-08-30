using Darmon.Application.DTOs.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.OrderDtos;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;

    public int UserId { get; set; }
    public int? DeliveryId { get; set; }
    public int? PaymentId { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new();
}
