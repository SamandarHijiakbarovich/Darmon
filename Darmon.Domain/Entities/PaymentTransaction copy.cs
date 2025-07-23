using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

internal class PaymentTransaction : BaseEntity
{
    public string GatewayTransactionId { get; set; }
    public string GatewayName { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    
    public int PaymentId { get; set; }
    public Payment Payment { get; set; }
}