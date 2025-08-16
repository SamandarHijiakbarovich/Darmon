// PaymentTransactionService.cs
using AutoMapper;
using Darmon.Application.DTOs.GatewayTransactionDtos;
using Darmon.Application.DTOs.PaymentTransactionDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Darmon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly IPaymentTransactionRepository _repository;
    private readonly IMapper _mapper;

    public PaymentTransactionService(IPaymentTransactionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaymentTransactionDto> InitTransactionAsync(InitTransactionDto dto)
    {
        var transaction = _mapper.Map<PaymentTransaction>(dto);
        transaction.Status = TransactionStatus.Created;
        transaction.InternalTraceId = Guid.NewGuid().ToString();
        transaction.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(transaction);
        await _repository.SaveChangesAsync();
        return _mapper.Map<PaymentTransactionDto>(transaction);
    }

    public async Task<PaymentTransactionDto> GetTransactionAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        return transaction == null ? null : _mapper.Map<PaymentTransactionDto>(transaction);
    }

    public async Task<PaymentTransactionDto> RetryTransactionAsync(RetryTransactionDto dto)
    {
        var existingTransaction = await _repository.GetByIdAsync(dto.TransactionId);
        if (existingTransaction == null)
            return null;

        existingTransaction.Status = TransactionStatus.Retry;
        existingTransaction.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingTransaction);
        return _mapper.Map<PaymentTransactionDto>(existingTransaction);
    }

    public async Task HandleCallbackAsync(GatewayCallbackDto dto)
    {
        var transaction = await _repository.GetByInternalTraceIdAsync(dto.InternalTraceId);
        if (transaction == null)
            return;

        transaction.Status = dto.Status;
        transaction.UpdatedAt = DateTime.UtcNow;

        // Map gateway response details
        _mapper.Map(dto, transaction);

        await _repository.UpdateAsync(transaction);
    }
}