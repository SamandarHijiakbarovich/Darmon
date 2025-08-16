using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ProductReviewDtos;

public class ProductReviewDto
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }

    public int UserId { get; set; }
    public string UserName { get; set; } // optional
}
