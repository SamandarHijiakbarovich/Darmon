using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class ProductImage:BaseEntity
{
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }

        // Relations
        public int ProductId { get; set; }
        public Product Product { get; set; }
}
