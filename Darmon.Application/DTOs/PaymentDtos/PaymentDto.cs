// PaymentDtos.cs
using Darmon.Application.DTOs.PaymentTransactionDtos;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Darmon.Application.DTOs.PaymentDtos;

public record PrepareRequestDto(
    string ClickTransId,
    string ServiceId,
    string MerchantTransId,
    string SignTime,
    string SignString,
    string Amount,
    string Action,
    string Error,
    string MerchantPrepareId,
    string ClickPayDocId

);

public record CompleteRequestDto(
    string ClickTransId,
    string ServiceId,
    string MerchantTransId,
    string SignTime,
    string SignString,
    string Amount,
    string Action,
    string Error,
    string MerchantPrepareId,
    string ClickPayDocId
) : PrepareRequestDto(
    ClickTransId,
    ServiceId,
    MerchantTransId,
    SignTime,
    SignString,
    Amount,
    Action,
    Error,
    MerchantPrepareId,
    ClickPayDocId
);

public record ClickResponseDto(
    int Error = 0,
    string ErrorNote = "",
    string MerchantTransId = "",
    string MerchantPrepareId = ""
);
