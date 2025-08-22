using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Darmon.Application.DTOs.CartItemDtos;
using Darmon.Domain.Entities.Enums;
using Darmon.Infrastructure.Repositories;

namespace Darmon.Application.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CartItemService> _logger;

    public CartItemService(
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CartItemService> logger)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CartItemDto>> GetUserCartItemsAsync(int userId)
    {
        try
        {
            var cartItems = await _cartItemRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart items for user {UserId}", userId);
            throw;
        }
    }

    public async Task<CartItemDto> AddCartItemAsync(int userId, CreateCartItemDto dto)
    {
        try
        {
            // Product mavjudligini tekshirish
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {dto.ProductId} not found");
            }

            // Miqdor tekshirish
            if (dto.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            // Stock tekshirish
            if (product.StockQuantity < dto.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock. Available: {product.StockQuantity}");
            }

            // Mavjud cart itemni tekshirish
            var existingItem = await _cartItemRepository.GetByUserAndProductAsync(userId, dto.ProductId);

            if (existingItem != null)
            {
                // Yangi miqdor stockdan oshmasligini tekshirish
                if (product.StockQuantity < existingItem.Quantity + dto.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock. Available: {product.StockQuantity}");
                }

                existingItem.Quantity += dto.Quantity;
                await _cartItemRepository.UpdateAsync(existingItem);
                return _mapper.Map<CartItemDto>(existingItem);
            }
            else
            {
                // Yangi cart item yaratish
                var newCartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                var createdItem = await _cartItemRepository.AddAsync(newCartItem);
                return _mapper.Map<CartItemDto>(createdItem);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding cart item for user {UserId}", userId);
            throw;
        }
    }

    public async Task<CartItemDto> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity)
    {
        try
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            var cartItem = await _cartItemRepository.GetByIdAsync(itemId);
            if (cartItem == null || cartItem.UserId != userId)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            // Product stock tekshirish
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null || product.StockQuantity < quantity)
            {
                throw new InvalidOperationException($"Not enough stock. Available: {product?.StockQuantity ?? 0}");
            }

            cartItem.Quantity = quantity;
            await _cartItemRepository.UpdateAsync(cartItem);

            return _mapper.Map<CartItemDto>(cartItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item {ItemId} for user {UserId}", itemId, userId);
            throw;
        }
    }

    public async Task<bool> RemoveCartItemAsync(int userId, int itemId)
    {
        try
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(itemId);
            if (cartItem == null || cartItem.UserId != userId)
            {
                return false;
            }

            await _cartItemRepository.DeleteAsync(itemId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item {ItemId} for user {UserId}", itemId, userId);
            throw;
        }
    }

    public async Task<bool> ClearUserCartAsync(int userId)
    {
        try
        {
            await _cartItemRepository.DeleteByUserIdAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
            throw;
        }
    }


    public async Task<CartItemDto> GetCartSummaryAsync(int userId)
    {
        try
        {
            var cartItems = await _cartItemRepository.GetByUserIdAsync(userId);
            var itemDtos = _mapper.Map<List<CartItemDto>>(cartItems);

            return new CartItemDto
            {
                TotalItems = cartItems.Sum(item => item.Quantity),
                TotalPrice = cartItems.Sum(item => item.Product.Price * item.Quantity),
                Items = itemDtos
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart summary for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> GetUserCartItemsCountAsync(int userId)
    {
        try
        {
            return await _cartItemRepository.GetCountByUserIdAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart items count for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> CheckProductInCartAsync(int userId, int productId)
    {
        try
        {
            var cartItem = await _cartItemRepository.GetByUserAndProductAsync(userId, productId);
            return cartItem != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking product {ProductId} in cart for user {UserId}", productId, userId);
            throw;
        }
    }
}