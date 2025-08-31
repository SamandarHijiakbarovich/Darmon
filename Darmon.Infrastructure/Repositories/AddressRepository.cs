using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<AddressRepository> _logger;

    public AddressRepository(AppDbContext context, ILogger<AddressRepository> logger)
        : base(context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Address>> GetByUserIdAsync(int userId)
    {
        try
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving addresses for user {UserId}", userId);
            throw;
        }
    }

    public async Task<Address?> GetDefaultAddressAsync(int userId)
    {
        try
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving default address for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> SetDefaultAddressAsync(int addressId, int userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var userAddresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            foreach (var address in userAddresses)
            {
                address.IsDefault = false;
            }

            var targetAddress = userAddresses.FirstOrDefault(a => a.Id == addressId);
            if (targetAddress == null)
            {
                throw new KeyNotFoundException($"Address with ID {addressId} not found for user {userId}");
            }

            targetAddress.IsDefault = true;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error setting default address {AddressId} for user {UserId}", addressId, userId);
            throw;
        }
    }

    public async Task<IEnumerable<Address>> SearchAddressesAsync(int userId, string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetByUserIdAsync(userId);
            }

            return await _context.Addresses
                .Where(a => a.UserId == userId &&
                           (a.Street.Contains(searchTerm) ||
                            a.City.Contains(searchTerm)))
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching addresses for user {UserId} with term {SearchTerm}", userId, searchTerm);
            throw;
        }
    }
}
