using AutoMapper;
using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Halpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class PaymentService:IClickPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public PaymentService(IPaymentRepository repository, IMapper mapper,
        IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        var merchantUserId = _config["Click:MerchantUserId"];
        var secretKey = _config["Click:SecretKey"];
        var authHeader = AuthHeaderGenerator.Generate(merchantUserId, secretKey);

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("Auth", authHeader);

        var response = await _httpClient.PostAsJsonAsync("https://api.click.uz/v2/merchant/invoice/create", dto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<InvoiceResponseDto>();
        return result!;
    }
    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto dto)
    {
        var payment = _mapper.Map<Payment>(dto);
        payment.Status = PaymentStatus.Pending;
        payment.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(payment);
        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<PaymentDto> GetPaymentAsync(Guid id)
    {
        var payment = await _repository.GetByIdAsync(id);
        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task UpdatePaymentStatusAsync(Guid paymentId, UpdatePaymentStatusDto dto)
    {
        var payment = await _repository.GetByIdAsync(paymentId);
        if (payment != null)
        {
            payment.Status = dto.Status;
            payment.Description = dto.Reason;
            await _repository.UpdateAsync(payment);
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(Guid orderId)
    {
        var payments = await _repository.GetByOrderIdAsync(orderId);
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }
}
