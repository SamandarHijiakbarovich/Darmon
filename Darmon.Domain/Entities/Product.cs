using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

public class Product:AuditableEntity
{
       public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }

        // Relations
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<ProductImage> Images { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<ProductReview> Reviews { get; set; }

}
