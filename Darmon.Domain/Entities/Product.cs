using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class Product:BaseEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string ImagePath { get; set; }

    public bool Available { get; set; } = true;

    public ProductType ProductType { get; set; }

    public int? CategoryId { get; set; }
    public Category Category { get; set; }

    public string Manufacturer { get; set; }
    public string Dosage { get; set; }
    public string Ingredients { get; set; }

    public Guid CreatedById { get; set; }
    public CustomUser CreatedBy { get; set; }

    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

}
