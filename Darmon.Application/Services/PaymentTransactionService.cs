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

    public async Task<PaymentTransaction> CreateTransactionAsync(PaymentTransactionDto dto)
    {
        var entity = _mapper.Map<PaymentTransaction>(dto);
        entity.Status = TransactionStatus.Created;
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return entity;
    }

    public async Task<PaymentTransactionDto> GetTransactionByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<PaymentTransactionDto>(entity);
    }

    public async Task<PaymentTransactionDto> UpdateTransactionAsync(PaymentTransactionDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity == null)
            return null;

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<PaymentTransactionDto>(entity);
    }

    public async Task<IEnumerable<PaymentTransactionDto>> GetTransactionsByPaymentIdAsync(Guid paymentId)
    {
        var entities = await _repository.GetByPaymentIdAsync(paymentId);
        return _mapper.Map<IEnumerable<PaymentTransactionDto>>(entities);
    }

    public async Task<IEnumerable<PaymentTransactionDto>> GetTransactionsByStatusAsync(TransactionStatus status, int pageNumber = 1, int pageSize = 10)
    {
        var entities = await _repository.GetByStatusAsync(status, pageNumber, pageSize);
        return _mapper.Map<IEnumerable<PaymentTransactionDto>>(entities);
    }

    public async Task<PaymentTransactionDto> GetByInternalTraceIdAsync(string internalTraceId)
    {
        var entity = await _repository.GetByInternalTraceIdAsync(internalTraceId);
        return entity == null ? null : _mapper.Map<PaymentTransactionDto>(entity);
    }

    public async Task<bool> DeleteTransactionAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        // You can return or log them if needed
    }
}