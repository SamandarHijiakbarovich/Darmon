using Darmon.Domain.Entities;
using System.Linq.Expressions;

namespace Darmon.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    // 🔍 Search & Filter
    Task<IEnumerable<Product>> SearchAsync(string keyword);                      // Nomi yoki tavsifi bo‘yicha izlash
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);              // Kategoriya bo‘yicha

    // 📦 Stock
    Task<IEnumerable<Product>> GetOutOfStockAsync();                            // Tugagan mahsulotlar
    Task<IEnumerable<Product>> GetLowStockAsync(int threshold);                 // Kam miqdordagi mahsulotlar

    // 🧠 Custom filter
    Task<IEnumerable<Product>> FilterAsync(Expression<Func<Product, bool>> predicate); // Har qanday shart bo‘yicha
}
