using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CartItemDtos;

public class UpdateCartItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}
