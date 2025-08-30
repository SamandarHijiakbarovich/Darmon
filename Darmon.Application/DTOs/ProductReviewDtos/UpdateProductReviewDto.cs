using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ProductReviewDtos;

public class UpdateProductReviewDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
