using AutoMapper;
using Darmon.Application.DTOs.DeliveryDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryService> _logger;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IOrderRepository orderRepository,
            IAddressRepository addressRepository,
            IBranchRepository branchRepository,
            IMapper mapper,
            ILogger<DeliveryService> logger)
        {
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // 1. Yangi yetkazib berish yaratish
        public async Task<DeliveryDto> CreateDeliveryAsync(CreateDeliveryDto dto)
        {
            try
            {
                // Tekshirishlar
                var order = await _orderRepository.GetByIdAsync(dto.OrderId);
                if (order == null) throw new ArgumentException("Order topilmadi");

                var address = await _addressRepository.GetByIdAsync(dto.DeliveryAddressId);
                if (address == null) throw new ArgumentException("Manzil topilmadi");

                // Yetkazib berish narxini hisoblash
                var deliveryFee = await CalculateDeliveryFeeAsync(dto.DeliveryAddressId, order.BranchId);

                var delivery = new Delivery
                {
                    OrderId = dto.OrderId,
                    AddressId = dto.DeliveryAddressId,
                    DeliveryFee = deliveryFee,
                    Status = DeliveryStatus.Pending,
                    EstimatedDeliveryTime = DateTime.UtcNow.AddHours(2)
                };

                 await _deliveryRepository.AddAsync(delivery);
                return _mapper.Map<DeliveryDto>(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateDeliveryAsync xatosi: OrderId={OrderId}", dto.OrderId);
                throw;
            }
        }

        // 2. ID bo'yicha yetkazib berishni olish
        public async Task<DeliveryDto> GetDeliveryByIdAsync(int id)
        {
            try
            {
                var delivery = await _deliveryRepository.GetByIdAsync(id);
                if (delivery == null) throw new KeyNotFoundException("Yetkazib berish topilmadi");

                return _mapper.Map<DeliveryDto>(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDeliveryByIdAsync xatosi: DeliveryId={DeliveryId}", id);
                throw;
            }
        }

        // 3. Order ID bo'yicha yetkazib berishni olish
        public async Task<DeliveryDto> GetDeliveryByOrderIdAsync(int orderId)
        {
            try
            {
                var delivery = await _deliveryRepository.GetByOrderIdAsync(orderId);
                if (delivery == null) throw new KeyNotFoundException("Buyurtma uchun yetkazib berish topilmadi");

                return _mapper.Map<DeliveryDto>(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDeliveryByOrderIdAsync xatosi: OrderId={OrderId}", orderId);
                throw;
            }
        }

        // 4. Yetkazib berish statusini yangilash
        public async Task<DeliveryDto> UpdateDeliveryStatusAsync(int deliveryId, UpdateDeliveryDto dto)
        {
            try
            {
                var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
                if (delivery == null) throw new KeyNotFoundException("Yetkazib berish topilmadi");

                delivery.Status = dto.Status;

                // Agar yetkazib berilgan bo'lsa, vaqtni belgilash
                if (dto.Status == DeliveryStatus.Delivered)
                    delivery.ActualDeliveryTime = DateTime.UtcNow;

                await _deliveryRepository.UpdateAsync(delivery);
                return _mapper.Map<DeliveryDto>(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDeliveryStatusAsync xatosi: DeliveryId={DeliveryId}", deliveryId);
                throw;
            }
        }

        // 5. Kurierni tayinlash
        public async Task<bool> AssignCourierAsync(int deliveryId, int courierId)
        {
            try
            {
                var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
                if (delivery == null) throw new KeyNotFoundException("Yetkazib berish topilmadi");

                delivery.CourierId = courierId;
                delivery.Status = DeliveryStatus.Accepted;

                await _deliveryRepository.UpdateAsync(delivery);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AssignCourierAsync xatosi: DeliveryId={DeliveryId}, CourierId={CourierId}", deliveryId, courierId);
                throw;
            }
        }

        // 6. Faol yetkazib berishlarni olish
        public async Task<IEnumerable<DeliveryDto>> GetActiveDeliveriesAsync()
        {
            try
            {
                var deliveries = await _deliveryRepository.GetActiveDeliveriesAsync();
                return _mapper.Map<IEnumerable<DeliveryDto>>(deliveries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetActiveDeliveriesAsync xatosi");
                throw;
            }
        }

        // 7. Yetkazib berish narxini hisoblash
        public async Task<decimal> CalculateDeliveryFeeAsync(int addressId, int branchId)
        {
            try
            {
                // Oddiy fixed narx - keyinroq murakkab hisoblash qo'shish mumkin
                return 15000m;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CalculateDeliveryFeeAsync xatosi: AddressId={AddressId}, BranchId={BranchId}", addressId, branchId);
                return 15000m; // Default narx
            }
        }
    }
}