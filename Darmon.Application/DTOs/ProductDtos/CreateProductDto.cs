using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ProductDTos;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public bool IsPrescriptionRequired { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public int? BranchId { get; set; }
}
