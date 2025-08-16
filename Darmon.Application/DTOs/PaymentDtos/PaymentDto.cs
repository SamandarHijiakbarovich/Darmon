// PaymentDtos.cs
using Darmon.Application.DTOs.PaymentTransactionDtos;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Darmon.Application.DTOs.PaymentDtos;

public record CreatePaymentDto(
    decimal Amount,
    Guid OrderId,
    string? Description = null,
    string Currency = "UZS",
    DateTime? ExpirationDate = null);

public record PaymentDto(
    Guid Id,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    string? Description,
    DateTime CreatedAt,
    DateTime? ExpirationDate,
    IEnumerable<PaymentTransactionDto> Transactions);

public record UpdatePaymentStatusDto(PaymentStatus Status, string? Reason);