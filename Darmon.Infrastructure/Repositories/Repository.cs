using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;


public class Repository<T>:IRepository<T> where T:class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();  // DbSet<T> ni olish
    }

    // CREATE
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // READ
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);

    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // UPDATE
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    // DELETE
    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            await DeleteAsync(entity);
    }

    // EXISTS
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    // TRANSACTIONS
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    // SAVE CHANGES
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking();
    }
}
    