using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories
{
    public class BranchRepository:Repository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BranchRepository> _logger;

        public BranchRepository(AppDbContext context, ILogger<BranchRepository> logger):base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // IRepository metodlari
        public async Task AddAsync(Branch entity)
        {
            try
            {
                await _context.Branches.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding branch: {BranchName}", entity.Name);
                throw;
            }
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Address)
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branch with ID {BranchId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(Branch entity)
        {
            try
            {
                _context.Branches.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating branch with ID {BranchId}", entity.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Branch entity)
        {
            try
            {
                _context.Branches.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting branch with ID {BranchId}", entity.Id);
                throw;
            }
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Address)
                    .Where(b => b.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all branches");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Branch, bool>> predicate)
        {
            try
            {
                return await _context.Branches.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking branch existence");
                throw;
            }
        }

        // IBranchRepository metodlari

        public async Task<IEnumerable<Branch>> GetActivePharmaciesAsync()
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Address)
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active branches");
                throw;
            }
        }

        public async Task<IEnumerable<Branch>> GetNearbyPharmaciesAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                // Soddalashtirilgan versiya - faqat aktiv filiallarni qaytaradi
                // Haqiqiy loyihada GPS masofasini hisoblash kerak
                return await _context.Branches
                    .Include(b => b.Address)
                    .Where(b => b.IsActive && b.Address != null)
                    .OrderBy(b => b.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving nearby branches");
                throw;
            }
        }

        public async Task<IEnumerable<Branch>> SearchPharmaciesAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetActivePharmaciesAsync();

                return await _context.Branches
                    .Include(b => b.Address)
                    .Where(b => b.IsActive &&
                               (b.Name.Contains(searchTerm) ||
                                (b.Address != null && b.Address.Street.Contains(searchTerm)) ||
                                (b.Address != null && b.Address.City.Contains(searchTerm)) ||
                                b.PhoneNumber.Contains(searchTerm)))
                    .OrderBy(b => b.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching branches with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Branch>> GetPharmaciesByWorkingHoursAsync(TimeSpan time)
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Address)
                    .Where(b => b.IsActive &&
                               b.OpeningTime <= time &&
                               b.ClosingTime >= time)
                    .OrderBy(b => b.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branches by working hours");
                throw;
            }
        }

        public async Task<Branch?> GetPharmacyWithProductsAsync(int branchId)
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Address)
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == branchId && b.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branch with products for ID {BranchId}", branchId);
                throw;
            }
        }

        public async Task<bool> IsPharmacyOpenAsync(int branchId, DateTime dateTime)
        {
            try
            {
                var branch = await GetByIdAsync(branchId);
                if (branch == null) return false;

                var time = dateTime.TimeOfDay;
                return time >= branch.OpeningTime && time <= branch.ClosingTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if branch {BranchId} is open", branchId);
                throw;
            }
        }

        public async Task<bool> CheckProductAvailabilityAsync(int branchId, int productId, int quantity)
        {
            try
            {
                var branch = await _context.Branches
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == branchId && b.IsActive);

                if (branch == null) return false;

                var product = branch.Products.FirstOrDefault(p => p.Id == productId);
                return product != null && product.StockQuantity >= quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking product availability in branch {BranchId}", branchId);
                throw;
            }
        }

        public async Task<Dictionary<int, int>> GetProductStockAsync(int branchId)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.BranchId == branchId)
                    .ToDictionaryAsync(p => p.Id, p => p.StockQuantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product stock for branch {BranchId}", branchId);
                throw;
            }
        }

        // Transaction metodlari
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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                var branch = await GetByIdAsync(id);
                if (branch != null)
                {
                    await DeleteAsync(branch);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting branch with ID {BranchId}", id);
                throw;
            }
        }

        public IQueryable<Branch> GetAll()
        {
            return _context.Branches.AsQueryable();
        }

        // Qo'shimcha foydali metodlar
        public async Task<IEnumerable<Branch>> GetBranchesWithLowStockAsync(int threshold = 5)
        {
            try
            {
                return await _context.Branches
                    .Include(b => b.Products)
                    .Where(b => b.IsActive &&
                               b.Products.Any(p => p.StockQuantity <= threshold))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branches with low stock");
                throw;
            }
        }

        public async Task<bool> UpdateBranchStockAsync(int branchId, int productId, int quantityChange)
        {
            try
            {
                var branch = await GetPharmacyWithProductsAsync(branchId);
                if (branch == null) return false;

                var product = branch.Products.FirstOrDefault(p => p.Id == productId);
                if (product == null) return false;

                product.StockQuantity += quantityChange;
                if (product.StockQuantity < 0) product.StockQuantity = 0;

                await UpdateAsync(branch);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock for product {ProductId} in branch {BranchId}", productId, branchId);
                throw;
            }
        }
    }
}
