using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities.Common;

internal abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Har bir entitiy uchun noyob ID

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
