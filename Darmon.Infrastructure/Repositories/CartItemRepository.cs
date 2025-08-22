using Darmon.Domain.Entities.Enums;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public class CartItemRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CartItemRepository> _logger;

    public CartItemRepository(AppDbContext context, ILogger<CartItemRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CartItem> AddAsync(CartItem cartItem)
    {
        try
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding cart item for user {UserId}", cartItem.UserId);
            throw;
        }
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.CartItems
                .Include(ci => ci.Product)  // Product ma'lumotlarini ham olish
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart item with ID {CartItemId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(CartItem cartItem)
    {
        try
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item with ID {CartItemId}", cartItem.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var cartItem = await GetByIdAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting cart item with ID {CartItemId}", id);
            throw;
        }
    }

    public async Task<List<CartItem>> GetByUserIdAsync(int userId)
    {
        try
        {
            return await _context.CartItems
                .Include(ci => ci.Product)  // Product ma'lumotlari
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart items for user {UserId}", userId);
            throw;
        }
    }

    public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId)
    {
        try
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cart item for user {UserId} and product {ProductId}", userId, productId);
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(int userId)
    {
        try
        {
            var cartItems = await GetByUserIdAsync(userId);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> GetCountByUserIdAsync(int userId)
    {
        try
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart count for user {UserId}", userId);
            throw;
        }
    }
}
