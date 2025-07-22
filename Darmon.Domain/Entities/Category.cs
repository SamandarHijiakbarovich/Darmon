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
    internal class Category:BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType Type { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
