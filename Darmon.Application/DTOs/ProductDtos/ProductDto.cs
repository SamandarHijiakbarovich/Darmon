using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ProductDTos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsPrescriptionRequired { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } // optional

    public int? BranchId { get; set; }
    public string BranchName { get; set; } // optional

    public List<string> ImageUrls { get; set; } // optional
    public double AverageRating { get; set; } // optional
}
