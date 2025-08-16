using Darmon.Domain.Entities.Common;

namespace Darmon.Domain.Entities;

public class ClickTransaction : AuditableEntity
{
    public string Provider { get; set; } = default!; // "Click", "Payme", "Uzcard"
    public string Status { get; set; } = default!;
    public string ProviderTransactionId { get; set; } = default!;
    public string TraceId { get; set; } = default!; // ✅ Qo‘shildi
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RawRequest { get; set; }
    public string? RawResponse { get; set; }
    public DateTime? ProviderTimestamp { get; set; }

    // Relations
    public int PaymentTransactionId { get; set; }
    public PaymentTransaction PaymentTransaction { get; set; } = default!;
}
