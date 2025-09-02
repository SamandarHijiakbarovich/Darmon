using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IPaymentService
{
    /*// Asosiy CRUD operatsiyalari
    Task<Payment> GetPaymentByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllPaymentsAsync(int pageNumber = 1, int pageSize = 10);
    Task<Payment> CreatePaymentAsync(CreatePaymentDto createPaymentDto);
    Task UpdatePaymentAsync(int id, UpdatePaymentDto updatePaymentDto);
    Task<bool> DeletePaymentAsync(int id);

    // Order bilan ishlash
    Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<Payment> CreatePaymentForOrderAsync(int orderId, CreatePaymentDto createPaymentDto);

    // Status operatsiyalari
    Task UpdatePaymentStatusAsync(int id, PaymentStatus status);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status, int pageNumber = 1, int pageSize = 10);

    // Click integrasiyasi
    Task<Payment> ProcessClickPrepareAsync(ClickPrepareRequestDto request);
    Task<Payment> ProcessClickCompleteAsync(ClickCompleteRequestDto request);
    Task<Payment> GetPaymentByMerchantTransIdAsync(int merchantTransId);

    // ✅ Yangi metodlar
    Task UpdateClickPaymentStatusAsync(int merchantTransId, bool isSuccess);

    // Payme integrasiyasi
    Task<Payment> ProcessPaymeCreateAsync(PaymeCreateRequestDto request);
    Task<Payment> ProcessPaymeCheckAsync(PaymeCheckRequestDto request);
    Task<Payment> ProcessPaymeConfirmAsync(PaymeConfirmRequestDto request);

    // Statistikalar
    Task<int> GetPaymentCountByStatusAsync(PaymentStatus status);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<PaymentStatus, int>> GetPaymentStatisticsAsync();

    // Validation va tekshirishlar
    Task<bool> ValidatePaymentAsync(int paymentId);
    Task<bool> CanProcessPaymentAsync(int paymentId);*/
}