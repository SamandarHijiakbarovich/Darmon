using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CategotyDtos;

public class CategoryDto
{
    public int Id { get; set; } // AuditableEntity'dan
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    // Optional: Mahsulotlar soni
    public int ProductCount { get; set; }
}
