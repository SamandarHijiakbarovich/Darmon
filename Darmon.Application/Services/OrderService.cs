using AutoMapper;
using Darmon.Application.DTOs.OrderDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class OrderService:IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var entity = _mapper.Map<Order>(dto);
        await _orderRepository.AddAsync(entity);
        await _orderRepository.SaveChangesAsync();
        return _mapper.Map<OrderDto>(entity);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var list = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(list);
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _orderRepository.DeleteAsync(id);
        await _orderRepository.SaveChangesAsync();
        return result;
    }
}
