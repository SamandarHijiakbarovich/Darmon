using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ProductImageDtos;

public class ProductImageDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }

    public int ProductId { get; set; }
}
