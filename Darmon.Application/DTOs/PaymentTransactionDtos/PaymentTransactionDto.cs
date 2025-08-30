using Darmon.Application.DTOs.GatewayTransactionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.PaymentTransactionDtos;

public record InitTransactionDto(
    int PaymentId,
    decimal Amount,
    string Gateway,
    string? CallbackUrl = null,
    string? ClientRedirectUrl = null);

public record PaymentTransactionDto(
    int Id,
    decimal Amount,
    string Status,
    string? ErrorMessage,
    DateTime CreatedAt,
    IEnumerable<GatewayTransactionDto> GatewayTransactions);

public record RetryTransactionDto(int TransactionId);