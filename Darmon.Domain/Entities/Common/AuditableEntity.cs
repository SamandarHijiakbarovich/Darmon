using Darmon.Domain.Entities.Common;

internal abstract class AuditableEntity : BaseEntity
{
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}