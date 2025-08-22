using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CartItemDtos;

public class CartItemDto
{
    public int Id { get; set; } // AuditableEntity'dan keladi
    public int ProductId { get; set; }
    public string ProductName { get; set; } // optional
    public decimal ProductPrice { get; set; } // optional

    public int UserId { get; set; }
    public int Quantity { get; set; }
    public int TotalItems { get; internal set; }
    public decimal TotalPrice { get; internal set; }
    public List<CartItemDto> Items { get; internal set; }
}
