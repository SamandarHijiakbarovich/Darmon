using Darmon.Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IClickPaymentService
{
    // 1. Asosiy to'lov operatsiyalari
    Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto request);
    Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(long paymentId);
    Task<PaymentStatusResponseDto> CheckStatusByMerchantTransIdAsync(string merchantTransId);
    Task<PaymentStatusResponseDto> CancelPaymentAsync(long paymentId);

    // 2. Karta tokeni bilan ishlash (agar kerak bo'lsa)
    Task<CardTokenResponseDto> CreateCardTokenAsync(CardTokenRequestDto request);
    Task<CardTokenPaymentResponseDto> PayWithCardTokenAsync(CardTokenPaymentDto request);
    Task<CardTokenVerifyResponseDto> VerifyCardTokenAsync(CardTokenVerifyDto request);

    // 3. Qo'shimcha operatsiyalar
    Task<FiscalSubmitResponseDto> SubmitFiscalDataAsync(FiscalSubmitRequestDto request);
}
