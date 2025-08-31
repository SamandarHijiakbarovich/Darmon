using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(Category category);
    Task SaveChangesAsync();
}
