using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Darmon.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // Generic filter
    public async Task<IEnumerable<Product>> FilterAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _context.Products
            .Where(predicate)
            .ToListAsync();
    }

    // Get products by branch
    public async Task<IEnumerable<Product>> GetByBranchAsync(int branchId)
    {
        return await _context.Products
            .Where(p => p.BranchId == branchId)
            .ToListAsync();
    }


    // Get products by category
    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }


    // Get products with low stock
    public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold)
    {
        return await _context.Products
            .Where(p => p.StockQuantity <= threshold)
            .ToListAsync();
    }

    // Get newest products
    public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int count = 10)
    {
        return await _context.Products
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    // Get out-of-stock products
    public async Task<IEnumerable<Product>> GetOutOfStockAsync()
    {
        return await _context.Products
            .Where(p => p.StockQuantity == 0)
            .ToListAsync();
    }

    // Search by name or description
    public async Task<IEnumerable<Product>> SearchAsync(string keyword)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
            .ToListAsync();
    }
}
