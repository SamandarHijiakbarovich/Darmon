using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.GatewayTransactionDtos;

public record GatewayTransactionDto(
    int Id,
    string Provider,
    string Status,
    string ProviderTransactionId,
    DateTime? ProviderTimestamp,
    string? ErrorCode);


public record GatewayCallbackDto
{
    public string TransactionId { get; set; } = default!; // Providerning tranzaksiya IDsi
    public string InternalTraceId { get; set; } = Guid.NewGuid().ToString();
    public string Gateway { get; set; } = default!; // "Payme", "Click" etc.
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; } = default!; // "success", "failed" etc.
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? MerchantData { get; set; } // Qo'shimcha ma'lumotlar
    public string Signature { get; set; } = default!; // Xavfsizlik imzosi

    // Providerga xos maydonlar (dynamic qabul qilish uchun)
    public Dictionary<string, object>? AdditionalData { get; set; }
}