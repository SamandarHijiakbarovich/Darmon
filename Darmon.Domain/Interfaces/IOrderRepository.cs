using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IOrderRepository:IRepository<Order>
{
    Task AddAsync(Order order);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task SaveChangesAsync();
}
