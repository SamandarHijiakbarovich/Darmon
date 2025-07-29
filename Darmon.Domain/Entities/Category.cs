using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities
{
    public class Category:AuditableEntity
    {
         public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Relations
        public ICollection<Product> Products { get; set; }
    }
}
