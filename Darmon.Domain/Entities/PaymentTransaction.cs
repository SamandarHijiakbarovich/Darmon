using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;


public class PaymentTransaction : BaseEntity
{
    public decimal Amount { get; set; }

    public string InternalTraceId { get; private set; } = Guid.NewGuid().ToString();

    public TransactionStatus Status { get; set; }

    public string? ClientRedirectUrl { get; set; }

    public string? CallbackUrl { get; set; }

    public string? ErrorMessage { get; set; }

    public string? GatewaySessionId { get; set; }

    // Relations
    public Guid PaymentId { get; set; }

    public Payment Payment { get; set; } = default!;

    public ClickTransaction ClickTransactions { get; set; } 

    // Helper methods
    public bool CanRetry(int retryWindowMinutes = 60)
        => Status == TransactionStatus.Failed &&
           CreatedAt.AddMinutes(retryWindowMinutes) > DateTime.UtcNow;
}


