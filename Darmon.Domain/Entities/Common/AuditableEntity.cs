using Darmon.Domain.Entities.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}