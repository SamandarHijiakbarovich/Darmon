using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories
{
    public class SellerWalletRepository : Repository<SellerWallet>, ISellerWalletRepository
    {
        private readonly AppDbContext _appDbContext;

        public SellerWalletRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<SellerWallet?> GetByUserIdAsync(int userId)
        {
            return await _appDbContext.SellerWallets
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<bool> UpdateBalanceAtomicAsync(int walletId, decimal amountDelta)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                // Lock bilan walletni olish (concurrency uchun)
                var wallet = await _appDbContext.SellerWallets
                    .FromSqlRaw("SELECT * FROM SellerWallets WITH (UPDLOCK) WHERE Id = {0}", walletId)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                    return false;

                // Balansni yangilash
                wallet.Balance += amountDelta;
                wallet.UpdatedAt = DateTime.UtcNow;

                // Database ga saqlash
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                throw new Exception($"Balansni yangilashda xatolik: {ex.Message}");
            }
        }

        public async Task<decimal> GetCurrentBalanceAsync(int walletId)
        {
            var wallet = await _appDbContext.SellerWallets
                .Where(w => w.Id == walletId)
                .Select(w => new { w.Balance })
                .FirstOrDefaultAsync();

            return wallet?.Balance ?? 0;
        }
    }
}