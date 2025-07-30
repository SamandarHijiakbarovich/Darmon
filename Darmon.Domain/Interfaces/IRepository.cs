using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    // Create
    Task AddAsync(T entity);

    // Read
    Task<T?> GetByIdAsync(int id);                  // Id bo'yicha olish (nullable)
    Task<IEnumerable<T>> GetAllAsync();             // Barchasini olish

    // Update
    Task UpdateAsync(T entity);                     // Yangilash

    // Delete
    Task DeleteAsync(T entity);                     // O'chirish
    Task DeleteByIdAsync(int id);                   // Id bo'yicha o'chirish

    // Exists
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);  // Mavjudligini tekshirish


    //Transactionlar 
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();


    Task<int> SaveChangesAsync();  // O'zgarishlarni saqlash
}
