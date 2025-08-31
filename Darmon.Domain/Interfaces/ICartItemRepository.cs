using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public interface ICartItemRepository
{
    Task<CartItem> AddAsync(CartItem cartItem);
    Task<CartItem?> GetByIdAsync(int id);
    Task UpdateAsync(CartItem cartItem);
    Task DeleteAsync(int id);

    // Yangi metodlar
    Task<List<CartItem>> GetByUserIdAsync(int userId);
    Task<CartItem?> GetByUserAndProductAsync(int userId, int productId);
    Task DeleteByUserIdAsync(int userId); // Savatni tozalash uchun
    Task<int> GetCountByUserIdAsync(int userId); // Savatdagi itemlar soni
}
