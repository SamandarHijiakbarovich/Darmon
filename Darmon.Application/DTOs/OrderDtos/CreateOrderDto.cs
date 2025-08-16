using Darmon.Application.DTOs.OrderItemDtos;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.OrderDtos;

public class CreateOrderDto
{
    public string OrderNumber { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }

    public int UserId { get; set; }
    public int? DeliveryId { get; set; }
    public int? PaymentId { get; set; }

    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}
