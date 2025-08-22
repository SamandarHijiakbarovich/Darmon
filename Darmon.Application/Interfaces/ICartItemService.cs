using Darmon.Application.DTOs.CartItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface ICartItemService
{
    Task<IEnumerable<CartItemDto>> GetUserCartItemsAsync(int userId);
    Task<CartItemDto> AddCartItemAsync(int userId, CreateCartItemDto dto);
    Task<CartItemDto> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity);
    Task<bool> RemoveCartItemAsync(int userId, int itemId);
    Task<bool> ClearUserCartAsync(int userId);
    Task<int> GetUserCartItemsCountAsync(int userId);
    Task<CartItemDto> GetCartSummaryAsync(int userId);
    Task<bool> CheckProductInCartAsync(int userId, int productId);
}
