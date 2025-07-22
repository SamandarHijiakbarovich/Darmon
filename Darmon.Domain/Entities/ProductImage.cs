using Darmon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities;

internal class ProductImage:BaseEntity
{
    public int Id { get; set; }

    // URL yoki fayl nomi (mahalliy yuklangan rasm bo‘lsa)
    public string ImageUrl { get; set; } = default!;

    // Product bilan bog‘liq (foreign key)
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;

    // Qachon yuklangan
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
